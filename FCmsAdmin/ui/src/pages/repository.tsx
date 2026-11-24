// Screens shared by the Pages, Content and Database sections.
import { useState, type ReactNode } from 'react';
import { Link, useNavigate, useParams } from 'react-router';
import { enumsApi, filtersApi, repositoryItemsApi, structureApi } from '../api/services';
import { ContentItemView, type ContentEditorContext } from '../components/content';
import { EditFilters, SearchFilters } from '../components/filters';
import { SlidePanel } from '../components/SlidePanel';
import { toast } from '../components/toast';
import { confirmDelete, EmptyState, IconButton, Loading, PageHeader } from '../components/ui';
import { keyOf, useLoad, useRerender } from '../hooks';
import {
  API_SUCCESS,
  type ContentDefinition,
  type ContentFilter,
  type ContentItem,
  type ContentList,
  type FilterDefinition,
  type PreviewItem,
  type RepositoryContent,
  type RepositoryStructure,
  type RepositorySummary,
  type Section,
} from '../models';

async function loadStructure(id: string): Promise<RepositoryStructure | undefined> {
  const result = await structureApi.get(id);
  if (result.status !== API_SUCCESS || !result.data) {
    toast.error(result.description || 'Repository not found');
    return undefined;
  }
  result.data.contentDefinitions ??= [];
  return result.data;
}

/** List of repositories with a row of action links each. */
export function RepositoryList({
  title,
  load,
  remove,
  addUrl,
  links,
}: {
  title: string;
  load: () => Promise<RepositorySummary[]>;
  remove: (id: string) => Promise<unknown>;
  addUrl: string;
  links: (id: string) => [label: string, url: string][];
}) {
  const { data: repositories, setData, loading } = useLoad(load, []);

  const deleteRow = async (repo: RepositorySummary) => {
    if (!confirmDelete(`"${repo.name}"`)) return;
    const id = repo.id!;
    try {
      await remove(id);
      setData((list) => list?.filter((r) => r.id !== id));
    } catch (e) {
      toast.fromError(e);
    }
  };

  return (
    <>
      <PageHeader
        title={title}
        actions={
          <Link to={addUrl} className="cms-button">
            <span className="fa fa-plus" /> Add
          </Link>
        }
      />
      {loading && !repositories && <Loading />}
      {repositories?.length === 0 && (
        <EmptyState>
          Nothing here yet. <Link to={addUrl}>Create the first one</Link>.
        </EmptyState>
      )}
      {repositories?.map((repo) => {
        const [primary, ...rest] = links(repo.id!);
        return (
          <div className="cms-repository" id={repo.id} key={repo.id}>
            <div className="cms-card-header">
              <h2>
                <Link to={primary[1]}>{repo.name}</Link>
              </h2>
              <IconButton icon="trash" title="Delete" danger onClick={() => deleteRow(repo)} />
            </div>
            <div className="cms-card-actions cmscontrols">
              {[primary, ...rest].map(([label, url], i) => (
                <Link key={url} to={url} className={`cms-button cms-button-sm${i > 0 ? ' cms-button-secondary' : ''}`}>
                  {label}
                </Link>
              ))}
            </div>
          </div>
        );
      })}
    </>
  );
}

/** Edits the content definitions of a repository. */
export function StructurePage({ section }: { section: Section }) {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const rerender = useRerender();
  const { data: model, loading } = useLoad(() => loadStructure(id), [id]);

  if (!model) return loading ? <Loading /> : null;

  const save = async () => {
    try {
      await structureApi.save(model);
      navigate(`/${section}`);
    } catch (e) {
      toast.fromError(e);
    }
  };

  const addValue = () => {
    model.contentDefinitions.push({ name: '', typeName: 'String', contentDefinitions: [] });
    rerender();
  };

  return (
    <>
      <PageHeader title={<>"{model.name}" structure</>} back={{ to: `/${section}`, label: 'Back to list' }} />
      {model.contentDefinitions.length === 0 && <EmptyState>No values defined yet. Add one below.</EmptyState>}
      {model.contentDefinitions.map((definition, index) => (
        <div id={definition.definitionId} className="cms-repository" key={definition.definitionId ?? keyOf(definition)}>
          <IconButton
            icon="times"
            title="Delete"
            danger
            className="cms-right"
            onClick={() => {
              model.contentDefinitions.splice(index, 1);
              rerender();
            }}
          />
          <ContentDefinitionEditor definition={definition} onChange={rerender} />
        </div>
      ))}
      <div className="cms-buttons">
        <button type="button" className="cms-button" onClick={save}>
          Save
        </button>
        <button type="button" className="cms-button cms-button-secondary" onClick={addValue}>
          <span className="fa fa-plus" /> Add value
        </button>
        <button type="button" className="cms-button cms-button-secondary" onClick={() => navigate(`/${section}`)}>
          Cancel
        </button>
      </div>
    </>
  );
}

function ContentDefinitionEditor({ definition, onChange }: { definition: ContentDefinition; onChange: () => void }) {
  const { data: enums } = useLoad(() => enumsApi.get(), []);
  const children = (definition.contentDefinitions ??= []);

  return (
    <div className="row g-3">
      <div className="col-md-6">
        <label>Name</label>
        <input
          className="form-control"
          value={definition.name ?? ''}
          onChange={(e) => {
            definition.name = e.target.value;
            onChange();
          }}
        />
      </div>
      <div className="col-md-5">
        <label>Type</label>
        <select
          className="form-control"
          value={definition.typeName}
          onChange={(e) => {
            definition.typeName = e.target.value;
            onChange();
          }}
        >
          {(enums?.contentDataTypes ?? [definition.typeName]).map((type) => (
            <option key={type} value={type}>
              {type}
            </option>
          ))}
        </select>
      </div>
      {definition.typeName === 'Folder' && (
        <div className="cms-folder">
          {children.map((child, index) => (
            <div id={child.definitionId} className="mb-3" key={child.definitionId ?? keyOf(child)}>
              <IconButton
                icon="times"
                title="Delete"
                danger
                className="cms-right"
                onClick={() => {
                  children.splice(index, 1);
                  onChange();
                }}
              />
              <ContentDefinitionEditor definition={child} onChange={onChange} />
            </div>
          ))}
          <div className="cms-right-controls">
            <button
              type="button"
              className="cms-button cms-button-secondary cms-button-sm"
              onClick={() => {
                children.push({ name: '', typeName: 'String', contentDefinitions: [] });
                onChange();
              }}
            >
              <span className="fa fa-plus" /> Add value
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

/** Edits the content values of a repository for the current set of filters. */
export function EditContentPage({ section }: { section: Section }) {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const rerender = useRerender();
  const [filters, setFilters] = useState<ContentFilter[]>([]);

  const { data, loading } = useLoad(async () => {
    const [content, definition] = await Promise.all([repositoryItemsApi.getContent(id, filters), loadStructure(id)]);
    if (!definition) return undefined;
    const model: RepositoryContent = content.data ?? { contentItems: [] };
    model.contentItems ??= [];
    return { definition, content: model };
  }, [id, filters]);

  if (!data) return loading ? <Loading /> : null;
  const { definition, content } = data;
  const ctx: ContentEditorContext = { section, repositoryId: id, onChange: rerender };

  const save = async () => {
    try {
      await repositoryItemsApi.save(content);
      navigate(`/${section}`);
    } catch (e) {
      toast.fromError(e);
    }
  };

  return (
    <>
      <PageHeader
        title={<>"{definition.name}" content</>}
        back={{ to: `/${section}`, label: 'Back to list' }}
        actions={
          <button type="button" className="cms-button" onClick={save}>
            <span className="fa fa-check" /> Save
          </button>
        }
      />
      <div className="row">
        <div className="col-md-8">
          {definition.contentDefinitions.length === 0 && (
            <EmptyState>
              This repository has no values yet. <Link to={`/${section}/structure/${id}`}>Edit its structure</Link>.
            </EmptyState>
          )}
          {definition.contentDefinitions.map((d) => (
            <div id={d.definitionId} className="cms-repository" key={d.definitionId ?? keyOf(d)}>
              <ContentItemView ctx={ctx} definition={d} data={content.contentItems} filters={filters} />
            </div>
          ))}
          <button type="button" className="cms-button" onClick={save}>
            Save
          </button>
        </div>
        <div className="col-md-4">
          <div className="cms-sidebar">
            <div className="cms-repository">
              <h3>Content sections</h3>
              <ul className="mb-0">
                {definition.contentDefinitions.map((d) => (
                  <li key={d.definitionId ?? keyOf(d)}>
                    <a href={`#${d.definitionId}`}>{d.name}</a>
                  </li>
                ))}
              </ul>
            </div>
            <div className="cms-repository">
              <h3>Filters</h3>
              <SearchFilters onFilter={setFilters} />
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

function formatFilterValue(definition: FilterDefinition, values: unknown[]): ReactNode {
  if (definition.type === 'DateRange') {
    const fmt = (v: unknown) => (v ? new Date(String(v)).toLocaleString() : '');
    return `${fmt(values[0])} - ${fmt(values[1])}`;
  }
  // Values may come back as JSON booleans/numbers, which React would not render.
  return String(values[0] ?? '');
}

/** Lists every variation (value + filters) of one content definition. */
export function ListContentPage({ section }: { section: Section }) {
  const { repo = '', id = '' } = useParams();
  const rerender = useRerender();
  const [searchFilters, setSearchFilters] = useState<ContentFilter[]>([]);
  const [editing, setEditing] = useState<{ index: number; snapshot: ContentItem } | null>(null);

  const { data, loading } = useLoad(async () => {
    const [filterDefinitions, list] = await Promise.all([
      filtersApi.list(),
      repositoryItemsApi.listContent(repo, id, searchFilters),
    ]);
    const content = list.data as ContentList;
    content.contentItems ??= [];
    return { filters: new Map(filterDefinitions.map((f) => [f.id, f])), content };
  }, [repo, id, searchFilters]);

  if (!data) return loading ? <Loading /> : null;
  const { content, filters } = data;
  const items = content.contentItems;
  const definition = content.definition;
  const ctx: ContentEditorContext = { section, repositoryId: repo, onChange: rerender };

  const remove = async (item: ContentItem) => {
    if (!item.id || !confirmDelete('this variation')) return;
    try {
      await repositoryItemsApi.remove(repo, item.id);
      items.splice(items.indexOf(item), 1);
      rerender();
    } catch (e) {
      toast.fromError(e);
    }
  };

  const saveEdit = async () => {
    if (!editing) return;
    try {
      await repositoryItemsApi.updateItem(repo, items[editing.index]);
      setEditing(null);
    } catch (e) {
      toast.fromError(e);
    }
  };

  const cancelEdit = () => {
    if (editing) items[editing.index] = editing.snapshot;
    setEditing(null);
  };

  const editedItem = editing ? items[editing.index] : undefined;

  return (
    <>
      <PageHeader
        title={<>Variations of "{definition?.name}"</>}
        back={{ to: `/${section}/edit/${repo}`, label: content.repositoryName ?? 'Back' }}
      />

      <div className="row">
        <div className="col-md-8">
          {items.length === 0 && <EmptyState>No variations match the search.</EmptyState>}
          {items.map((item, index) => (
            <div id={definition.definitionId} className="cms-repository" key={item.id ?? keyOf(item)}>
              <IconButton icon="times" title="Delete" danger className="cms-right" onClick={() => remove(item)} />
              <IconButton
                icon="edit"
                title="Edit"
                className="cms-right"
                onClick={() => setEditing({ index, snapshot: structuredClone(item) })}
              />
              {definition.typeName === 'Folder' ? (
                item.children?.map((child, i) => <div key={child.id ?? i}>{child.data}</div>)
              ) : (
                <>
                  <h3>Value</h3>
                  <span>{typeof item.data === 'string' ? item.data : JSON.stringify(item.data)}</span>
                </>
              )}
              <hr />
              <h3>Filters</h3>
              {!item.filters?.length && <span className="text-secondary">None — shown to everyone</span>}
              {item.filters?.map((filter) => {
                const filterDefinition = filters.get(filter.filterDefinitionId);
                if (!filterDefinition) return null;
                return (
                  <div key={filter.filterDefinitionId}>
                    <label>{filterDefinition.name}</label>:{' '}
                    <span>{formatFilterValue(filterDefinition, filter.values)}</span>
                  </div>
                );
              })}
            </div>
          ))}
        </div>
        <div className="col-md-4">
          <div className="cms-sidebar fcms-top-search">
            <h3>Search</h3>
            <SearchFilters onFilter={setSearchFilters} />
          </div>
        </div>
      </div>

      <SlidePanel isVisible={!!editedItem} onSave={saveEdit} onCancel={cancelEdit}>
        {editedItem && (
          <>
            <ContentItemView
              ctx={ctx}
              definition={definition}
              data={[editedItem]}
              isControlsVisible={false}
              isNewItemVisible={false}
            />
            <EditFilters model={editedItem.filters} onChange={rerender} />
          </>
        )}
      </SlidePanel>
    </>
  );
}

/** Shows what the repository would render for a set of filter values. */
export function PreviewPage() {
  const { id = '' } = useParams();
  const [filters, setFilters] = useState<ContentFilter[]>([]);
  const { data: definition } = useLoad(() => loadStructure(id), [id]);
  const { data: items } = useLoad(
    () => repositoryItemsApi.filterContent(id, filters).then((r) => (r.data ?? []) as PreviewItem[]),
    [id, filters],
  );

  return (
    <div className="row preview">
      <div className="col-md-8">
        <h1>{definition?.name} preview</h1>
        {items?.length === 0 && <EmptyState>Nothing would be shown for these filter values.</EmptyState>}
        {items?.map((item, i) => (
          <div className="cms-repository" key={i}>
            <label>{item.name}:</label>
            <div>{item.value}</div>
            {item.children != null && (
              <div className="cms-folder">
                {item.children.map((child, j) => (
                  <div className="fcms-forlder-item" key={j}>
                    <label>{child.name}:</label>
                    <div>{child.value}</div>
                  </div>
                ))}
              </div>
            )}
          </div>
        ))}
      </div>
      <div className="col-md-4">
        <div className="cms-sidebar cms-repository">
          <h3>Filter values</h3>
          <SearchFilters expanded onFilter={setFilters} />
        </div>
      </div>
    </div>
  );
}
