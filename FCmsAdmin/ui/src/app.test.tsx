import { act, cleanup, fireEvent, render, screen } from '@testing-library/react';
import { createMemoryRouter, MemoryRouter, RouterProvider } from 'react-router';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { clearToken, isLoggedIn, setToken } from './api/auth';
import { request, setUnauthorizedHandler } from './api/http';
import { routes } from './App';
import { ContentItemView, type ContentEditorContext } from './components/content';
import { dateInputToIso, isoToDateInput } from './components/inputs';
import type { ContentDefinition, ContentItem } from './models';

function jwt(expSecondsFromNow: number) {
  const payload = btoa(JSON.stringify({ exp: Math.floor(Date.now() / 1000) + expSecondsFromNow }));
  return `x.${payload}.y`;
}

afterEach(() => {
  cleanup();
  clearToken();
  vi.restoreAllMocks();
});

describe('auth', () => {
  it('treats only unexpired tokens as logged in', () => {
    expect(isLoggedIn()).toBe(false);
    setToken(jwt(-60));
    expect(isLoggedIn()).toBe(false);
    setToken(jwt(3600));
    expect(isLoggedIn()).toBe(true);
  });

  it('sends the bearer token, and on 401 clears it and calls the unauthorized handler', async () => {
    const token = jwt(3600);
    setToken(token);
    const handler = vi.fn();
    setUnauthorizedHandler(handler);
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('', { status: 401 }));

    await expect(request('GET', 'v1/config/filters')).rejects.toMatchObject({ status: 401 });

    const headers = fetchMock.mock.calls[0][1]!.headers as Record<string, string>;
    expect(headers.Authorization).toBe(`Bearer ${token}`);
    expect(handler).toHaveBeenCalledOnce();
    expect(isLoggedIn()).toBe(false);
  });

  it('redirects to login when not signed in', async () => {
    const router = createMemoryRouter(routes, { initialEntries: ['/config/filters'] });
    render(<RouterProvider router={router} />);
    expect(await screen.findByRole('heading', { name: 'Sign In' })).toBeTruthy();
    expect(router.state.location.search).toBe('?returnUrl=%2Fconfig%2Ffilters');
  });
});

describe('date inputs', () => {
  it('round-trips a local date through ISO', () => {
    expect(isoToDateInput(dateInputToIso('2024-03-09'))).toBe('2024-03-09');
    expect(dateInputToIso('')).toBe('');
    expect(isoToDateInput(null)).toBe('');
  });
});

describe('ContentItemView', () => {
  const stringDef: ContentDefinition = { definitionId: 'd1', name: 'Title', typeName: 'String', contentDefinitions: [] };
  const folderDef: ContentDefinition = {
    definitionId: 'f1',
    name: 'Links',
    typeName: 'Folder',
    contentDefinitions: [{ definitionId: 'c1', name: 'Url', typeName: 'String', contentDefinitions: [] }],
  };

  function renderView(definition: ContentDefinition, data: ContentItem[], filters = [] as ContentItem['filters']) {
    const ctx: ContentEditorContext = { section: 'pages', repositoryId: 'r1', onChange: () => view.rerender(ui()) };
    const ui = () => (
      <MemoryRouter>
        <ContentItemView ctx={ctx} definition={definition} data={data} filters={filters} />
      </MemoryRouter>
    );
    const view = render(ui());
    return view;
  }

  beforeEach(() => vi.spyOn(globalThis, 'fetch').mockResolvedValue(new Response('{"data":[]}')));

  it('adds a value carrying a copy of the current filters, then edits it in place', () => {
    const data: ContentItem[] = [];
    const filters = [{ filterDefinitionId: 'flt', filterType: 'Include', dataType: 'Text', values: ['a'] }];
    renderView(stringDef, data, filters);

    act(() => fireEvent.click(screen.getByText('Add value')));
    expect(data).toHaveLength(1);
    expect(data[0]).toMatchObject({ definitionId: 'd1', data: '', isFolder: false, filters });
    expect(data[0].filters).not.toBe(filters);

    act(() => fireEvent.change(screen.getByRole('textbox'), { target: { value: 'Hello' } }));
    expect(data[0].data).toBe('Hello');
  });

  it('adds folder items with a child per definition and soft-deletes saved ones', () => {
    const saved: ContentItem = {
      id: 'saved',
      definitionId: 'f1',
      isFolder: true,
      isDeleted: false,
      data: null,
      filters: [],
      children: [{ definitionId: 'c1', isFolder: false, isDeleted: false, data: 'x', filters: [], children: [] }],
    };
    const data: ContentItem[] = [saved];
    renderView(folderDef, data);

    act(() => fireEvent.click(screen.getByText('Add new value')));
    expect(data).toHaveLength(2);
    expect(data[1].children.map((c) => c.definitionId)).toEqual(['c1']);

    const deleteButtons = () => document.querySelectorAll('.cms-folder > .fa-times');
    expect(deleteButtons()).toHaveLength(2);
    act(() => fireEvent.click(deleteButtons()[0]));
    expect(saved.isDeleted).toBe(true);
    expect(data).toHaveLength(2);

    // An unsaved item has nothing to delete on the server, so it is just dropped.
    act(() => fireEvent.click(deleteButtons()[0]));
    expect(data).toEqual([saved]);
  });
});
