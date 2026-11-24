// Screens shared by the Pages, Content and Database sections.
import { useState, type ReactNode } from 'react';
import { Link, useNavigate, useParams } from 'react-router';
import { enumsApi, filtersApi, repositoryItemsApi, structureApi } from '../api/services';
import { ContentItemView, type ContentEditorContext } from '../components/content';
import { EditFilters, SearchFilters } from '../components/filters';
import { SlidePanel } from '../components/SlidePanel';
import { toast } from '../components/toast';
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
  const { data: repositories, setData } = useLoad(load, []);

  const deleteRow = async (id: string) => {
    try {
      await remove(id);
      setData((list) => list?.filter((r) => r.id !== id));
    } catch (e) {
      toast.fromError(e);
    }
  };

  return (
    <>
      <h1>{title}</h1>
      {repositories?.map((repo) => (
        <div className="cms-repository" id={repo.id} key={repo.id}>
          <h2>{repo.name}</h2>
          <div className="cms-subline"></div>
          <div className="cmscontrols">
            {links(repo.id!).map(([label, url]) => (
              <span key={url}>
                <Link to={url}>{label}</Link> |{' '}
              </span>
            ))}
            <a onClick={() => deleteRow(repo.id!)}>Delete</a>
          </div>
        </div>
      ))}
      <br />
      <Link to={addUrl} className="cms-button">
        Add
      </Link>
    </>
  );
}

/** Edits the content definitions of a repository. */
export function StructurePage({ section }: { section: Section }) {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const rerender = useRerender();
  const { data: model } = useLoad(() => loadStructure(id), [id]);

  if (!model) return null;

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
      <h1>"{model.name}" structure</h1>
      {model.contentDefinitions.map((definition, index) => (
        <div id={definition.definitionId} className="cms-repository" key={definition.definitionId ?? keyOf(definition)}>
          <div
            className="cms-right fa fa-times link"
            title="Delete"
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
        <button type="button" className="cms-button" onClick={addValue}>
          Add value
        </button>
        <button type="button" className="cms-button" onClick={() => navigate(`/${section}`)}>
          Back
        </button>
      </div>
    </>
  );
}

function ContentDefinitionEditor({ definition, onChange }: { definition: ContentDefinition; onChange: () => void }) {
  const { data: enums } = useLoad(() => enumsApi.get(), []);
  const children = (definition.contentDefinitions ??= []);

  return (
    <div className="row">
      <div className="col-md-5">
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
            <div id={child.definitionId} key={child.definitionId ?? keyOf(child)}>
              <div
                className="cms-right fa fa-times link"
                title="Delete"
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
              className="cms-button"
              onClick={() => {
                children.push({ name: '', typeName: 'String', contentDefinitions: [] });
                onChange();
              }}
            >
              Add value
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

  const { data } = useLoad(async () => {
    const [content, definition] = await Promise.all([repositoryItemsApi.getContent(id, filters), loadStructure(id)]);
    if (!definition) return undefined;
    const model: RepositoryContent = content.data ?? { contentItems: [] };
    model.contentItems ??= [];
    return { definition, content: model };
  }, [id, filters]);

  if (!data) return null;
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
      <h1>"{definition.name}" content</h1>
      <div className="row">
        <div className="col-md-8">
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
          <h2>Content sections:</h2>
          <ul>
            {definition.contentDefinitions.map((d) => (
              <li key={d.definitionId ?? keyOf(d)}>
                <a href={`#${d.definitionId}`}>{d.name}</a>
              </li>
            ))}
          </ul>
          <h2>Filters:</h2>
          <SearchFilters onFilter={setFilters} />
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

  const { data } = useLoad(async () => {
    const [filterDefinitions, list] = await Promise.all([
      filtersApi.list(),
      repositoryItemsApi.listContent(repo, id, searchFilters),
    ]);
    const content = list.data as ContentList;
    content.contentItems ??= [];
    return { filters: new Map(filterDefinitions.map((f) => [f.id, f])), content };
  }, [repo, id, searchFilters]);

  if (!data) return null;
  const { content, filters } = data;
  const items = content.contentItems;
  const definition = content.definition;
  const ctx: ContentEditorContext = { section, repositoryId: repo, onChange: rerender };

  const remove = async (item: ContentItem) => {
    if (!item.id) return;
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
      <h1>
        <Link to={`/${section}/edit/${repo}`}>{content.repositoryName}</Link> - "{definition?.name}"
      </h1>

      <div className="fcms-top-search col-md-8">
        <h3>Search</h3>
        <SearchFilters onFilter={setSearchFilters} />
      </div>

      <div className="row">
        <div className="col-md-8">
          {items.map((item, index) => (
            <div id={definition.definitionId} className="cms-repository" key={item.id ?? keyOf(item)}>
              <span className="cms-right fa fa-times link padding-left-8" title="Delete" onClick={() => remove(item)} />
              <span
                className="cms-right fa fa-edit link padding-left-8"
                title="Edit"
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
              {item.filters?.map((filter) => {
                const filterDefinition = filters.get(filter.filterDefinitionId);
                if (!filterDefinition) return null;
                return (
                  <div key={filter.filterDefinitionId}>
                    <label>{filterDefinition.name}</label>: <span>{formatFilterValue(filterDefinition, filter.values)}</span>
                  </div>
                );
              })}
            </div>
          ))}
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
        <h1>{definition?.name} Preview</h1>
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
        <h3>Search</h3>
        <SearchFilters expanded onFilter={setFilters} />
      </div>
    </div>
  );
}
