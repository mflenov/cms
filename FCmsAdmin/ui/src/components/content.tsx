import { useState } from 'react';
import { Link } from 'react-router';
import { keyOf } from '../hooks';
import type { ContentDefinition, ContentFilter, ContentItem, Section } from '../models';
import { EditFilters } from './filters';
import { DateInput, RichTextEditor } from './inputs';
import { SlidePanel } from './SlidePanel';
import { IconButton } from './ui';

function filtersLabel(filters: ContentFilter[] | undefined) {
  return filters?.length ? `Filters (${filters.length})` : 'Filters';
}

/** Context shared by every editor in one repository's content tree. */
export interface ContentEditorContext {
  section: Section;
  repositoryId: string;
  /** Called after the model was edited in place, so the owner re-renders. */
  onChange: () => void;
}

export function newFolderItem(definition: ContentDefinition): ContentItem {
  return {
    definitionId: definition.definitionId!,
    isFolder: true,
    isDeleted: false,
    filters: [],
    data: null,
    children: (definition.contentDefinitions ?? []).map((child) => ({
      definitionId: child.definitionId!,
      isFolder: false,
      isDeleted: false,
      filters: [],
      data: null,
      children: [],
    })),
  };
}

export function newValueItem(definition: ContentDefinition, filters: ContentFilter[]): ContentItem {
  return {
    definitionId: definition.definitionId!,
    isFolder: false,
    isDeleted: false,
    filters: structuredClone(filters),
    data: definition.typeName === 'String' ? '' : null,
    children: [],
  };
}

function listVariationsUrl(ctx: ContentEditorContext, definition: ContentDefinition) {
  return `/${ctx.section}/list/${ctx.repositoryId}/${definition.definitionId}`;
}

interface ContentItemViewProps {
  ctx: ContentEditorContext;
  definition: ContentDefinition;
  /** All content items of the repository (or of a folder item); new items are pushed into it. */
  data: ContentItem[];
  /** Filters given to items created here (the currently searched-for filter set). */
  filters?: ContentFilter[];
  /** True for a value inside a folder item. */
  folderItem?: boolean;
  isControlsVisible?: boolean;
  isNewItemVisible?: boolean;
}

/** Shows the editor for one content definition, or an "Add value" prompt when it has no value yet. */
export function ContentItemView({
  ctx,
  definition,
  data,
  filters = [],
  folderItem = false,
  isControlsVisible = true,
  isNewItemVisible = true,
}: ContentItemViewProps) {
  const isFolder = definition.typeName === 'Folder';
  const matches = data.filter((item) => item.definitionId === definition.definitionId && !item.isDeleted);

  const addValue = () => {
    if (isFolder) {
      const item = newFolderItem(definition);
      item.filters = structuredClone(filters);
      data.push(item);
    } else {
      data.push(newValueItem(definition, filters));
    }
    ctx.onChange();
  };

  if (matches.length > 0) {
    return (
      <ContentItemEditor
        ctx={ctx}
        definition={definition}
        items={matches}
        folderItemEditor={folderItem}
        isControlsVisible={isControlsVisible}
        onAddFolderItem={(item) => {
          data.push(item);
          ctx.onChange();
        }}
        onDeleteFolderItem={(item) => {
          if (item.id) item.isDeleted = true;
          else data.splice(data.indexOf(item), 1);
          ctx.onChange();
        }}
      />
    );
  }

  if (!isNewItemVisible) return null;
  return (
    <div>
      <h3>{definition.name}</h3>
      <span className="text-secondary">No value for these filters.</span>{' '}
      <button type="button" className="btn btn-link p-0 align-baseline cms-link" onClick={addValue}>
        Add value
      </button>
      {!folderItem && isControlsVisible && (
        <div className="cms-controls cms-buttons">
          <Link className="cms-button cms-button-secondary cms-button-sm" to={listVariationsUrl(ctx, definition)}>
            List variations
          </Link>
        </div>
      )}
    </div>
  );
}

interface ContentItemEditorProps {
  ctx: ContentEditorContext;
  definition: ContentDefinition;
  /** The value item (last one wins) or, for a folder, all folder items. */
  items: ContentItem[];
  folderItemEditor: boolean;
  isControlsVisible: boolean;
  onAddFolderItem: (item: ContentItem) => void;
  onDeleteFolderItem: (item: ContentItem) => void;
}

function ContentItemEditor({
  ctx,
  definition,
  items,
  folderItemEditor,
  isControlsVisible,
  onAddFolderItem,
  onDeleteFolderItem,
}: ContentItemEditorProps) {
  const [editingFilters, setEditingFilters] = useState<{ filters: ContentFilter[]; snapshot: ContentFilter[] } | null>(
    null,
  );
  const content = items[items.length - 1];

  const setData = (value: any) => {
    content.data = value;
    ctx.onChange();
  };

  const showFilters = (filters: ContentFilter[]) => setEditingFilters({ filters, snapshot: structuredClone(filters) });
  const cancelFilters = () => {
    if (editingFilters) editingFilters.filters.splice(0, editingFilters.filters.length, ...editingFilters.snapshot);
    setEditingFilters(null);
    ctx.onChange();
  };

  return (
    <>
      <h3>{definition.name}</h3>
      {definition.typeName === 'String' && (
        <input className="form-control" value={content.data ?? ''} onChange={(e) => setData(e.target.value)} />
      )}
      {definition.typeName === 'LongString' && <RichTextEditor value={content.data} onChange={setData} />}
      {(definition.typeName === 'Date' || definition.typeName === 'DateTime') && (
        <DateInput value={content.data} onChange={(iso) => setData(iso || null)} />
      )}

      {definition.typeName === 'Folder' && (
        <>
          {items.map((folderItem) => (
            <div className="cms-folder" key={folderItem.id ?? keyOf(folderItem)}>
              <IconButton
                icon="times"
                title="Delete"
                danger
                className="cms-right"
                onClick={() => onDeleteFolderItem(folderItem)}
              />
              {(definition.contentDefinitions ?? []).map((child) => (
                <div id={child.definitionId} className="fcms-forlder-item" key={child.definitionId ?? keyOf(child)}>
                  <ContentItemView ctx={ctx} folderItem definition={child} data={folderItem.children} />
                </div>
              ))}
              <div className="cms-controls">
                <button
                  type="button"
                  className="cms-button cms-button-secondary cms-button-sm"
                  onClick={() => showFilters(folderItem.filters)}
                >
                  <span className="fa fa-filter" /> {filtersLabel(folderItem.filters)}
                </button>
              </div>
            </div>
          ))}
          {isControlsVisible && (
            <div className="cms-right-controls cms-buttons">
              <button
                type="button"
                className="cms-button cms-button-secondary cms-button-sm"
                onClick={() => onAddFolderItem(newFolderItem(definition))}
              >
                Add new value
              </button>
              <Link className="cms-button cms-button-secondary cms-button-sm" to={listVariationsUrl(ctx, definition)}>
                List variations
              </Link>
            </div>
          )}
        </>
      )}

      {!folderItemEditor && definition.typeName !== 'Folder' && isControlsVisible && (
        <div className="cms-controls cms-buttons">
          <button
            type="button"
            className="cms-button cms-button-secondary cms-button-sm"
            onClick={() => showFilters(content.filters)}
          >
            <span className="fa fa-filter" /> {filtersLabel(content.filters)}
          </button>
          <Link className="cms-button cms-button-secondary cms-button-sm" to={listVariationsUrl(ctx, definition)}>
            List variations
          </Link>
        </div>
      )}

      <SlidePanel isVisible={editingFilters !== null} onSave={() => setEditingFilters(null)} onCancel={cancelFilters}>
        {editingFilters && <EditFilters model={editingFilters.filters} onChange={ctx.onChange} />}
      </SlidePanel>
    </>
  );
}
