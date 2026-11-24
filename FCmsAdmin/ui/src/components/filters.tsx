import type { ReactNode } from 'react';
import { useEffect, useRef, useState } from 'react';
import { filtersApi } from '../api/services';
import { useLoad, useRerender } from '../hooks';
import type { ContentFilter, FilterDefinition } from '../models';
import { DateInput } from './inputs';
import { IconButton } from './ui';

export function createFilterModel(definition: FilterDefinition): ContentFilter {
  return {
    filterDefinitionId: definition.id,
    filterType: 'Include',
    dataType: definition.type,
    values: [''],
  };
}

interface FilterEditorProps {
  definition: FilterDefinition;
  /** Edited in place; `onChange` is called after every change. */
  model: ContentFilter;
  /**
   * `editor` edits the filter attached to a content item (a DateRange takes a from/to range);
   * `search` is a search criterion (a DateRange takes the single date to match).
   */
  mode: 'editor' | 'search';
  onChange: () => void;
  onDelete: () => void;
}

/** One filter value editor; the control depends on the filter type. */
export function FilterEditor({ definition, model, mode, onChange, onDelete }: FilterEditorProps) {
  const setValue = (index: number, value: string) => {
    model.values[index] = value;
    onChange();
  };

  let control: ReactNode = null;
  switch (definition.type) {
    case 'Text':
    case 'RegEx':
      control = (
        <input className="form-control" value={model.values[0] ?? ''} onChange={(e) => setValue(0, e.target.value)} />
      );
      break;
    case 'Boolean':
      control = (
        <select className="form-control" value={model.values[0] ?? ''} onChange={(e) => setValue(0, e.target.value)}>
          <option value="">-</option>
          <option value="true">Yes</option>
          <option value="false">No</option>
        </select>
      );
      break;
    case 'ValueList':
      control = (
        <select className="form-control" value={model.values[0] ?? ''} onChange={(e) => setValue(0, e.target.value)}>
          <option value="">-</option>
          {(definition.values ?? []).map((v) => (
            <option key={v} value={v}>
              {v}
            </option>
          ))}
        </select>
      );
      break;
    case 'DateRange':
      control =
        mode === 'editor' ? (
          <>
            <div className="col-12 mb-1">
              <DateInput value={model.values[0]} onChange={(v) => setValue(0, v)} placeholder="From" />
            </div>
            <div className="col-12">
              <DateInput value={model.values[1]} onChange={(v) => setValue(1, v)} placeholder="To" />
            </div>
          </>
        ) : (
          <DateInput value={model.values[0]} onChange={(v) => setValue(0, v)} />
        );
      break;
    default:
      return null;
  }

  return (
    <div className="fcms-filter-item">
      <IconButton icon="times" title="Remove filter" danger className="cms-right" onClick={onDelete} />
      <h4>{definition.displayName}:</h4>
      {control}
    </div>
  );
}

function useFilterDefinitions() {
  const { data, loading } = useLoad(() => filtersApi.list(), []);
  return { definitions: data ?? [], loading };
}

/** Edits the filter list attached to a content item. `model` is edited in place. */
export function EditFilters({ model, onChange }: { model: ContentFilter[]; onChange?: () => void }) {
  const rerender = useRerender();
  const { definitions, loading } = useFilterDefinitions();
  const [selected, setSelected] = useState('');

  const changed = () => {
    rerender();
    onChange?.();
  };

  const used = new Set(model.map((f) => f.filterDefinitionId));
  const available = definitions.filter((d) => !used.has(d.id));
  const byId = new Map(definitions.map((d) => [d.id, d]));

  const add = () => {
    const definition = available.find((d) => d.id === selected);
    if (definition) model.push(createFilterModel(definition));
    setSelected('');
    changed();
  };

  const remove = (filter: ContentFilter) => {
    model.splice(model.indexOf(filter), 1);
    changed();
  };

  return (
    <div>
      <h2>Filters</h2>
      {loading ? (
        <div>Loading…</div>
      ) : (
        model.length === 0 && <p className="text-secondary">No filters — this value is shown to everyone.</p>
      )}
      {model.map((filter) => {
        const definition = byId.get(filter.filterDefinitionId);
        return definition ? (
          <FilterEditor
            key={filter.filterDefinitionId}
            definition={definition}
            model={filter}
            mode="editor"
            onChange={changed}
            onDelete={() => remove(filter)}
          />
        ) : null;
      })}
      {available.length > 0 && (
        <div className="d-flex gap-2 mt-3">
          <select className="form-control" value={selected} onChange={(e) => setSelected(e.target.value)}>
            <option value="">- select filter -</option>
            {available.map((d) => (
              <option key={d.id} value={d.id}>
                {d.name}
              </option>
            ))}
          </select>
          <button type="button" className="cms-button cms-button-secondary" disabled={!selected} onClick={add}>
            <span className="fa fa-plus" /> Add
          </button>
        </div>
      )}
    </div>
  );
}

/** Search criteria sidebar. Calls `onFilter` with a snapshot of the criteria when Search is clicked. */
export function SearchFilters({
  expanded = false,
  onFilter,
}: {
  /** Start with a criterion for every filter. */
  expanded?: boolean;
  onFilter: (filters: ContentFilter[]) => void;
}) {
  const rerender = useRerender();
  const { definitions } = useFilterDefinitions();
  const criteria = useRef<ContentFilter[]>([]);
  const expandedOnce = useRef(false);

  useEffect(() => {
    if (expanded && !expandedOnce.current && definitions.length > 0) {
      expandedOnce.current = true;
      criteria.current = definitions.map(createFilterModel);
      rerender();
    }
  }, [expanded, definitions, rerender]);

  const used = new Set(criteria.current.map((f) => f.filterDefinitionId));
  const available = definitions.filter((d) => !used.has(d.id));
  const byId = new Map(definitions.map((d) => [d.id, d]));

  const add = (id: string) => {
    const definition = available.find((d) => d.id === id);
    if (definition) {
      criteria.current.push(createFilterModel(definition));
      rerender();
    }
  };

  const remove = (filter: ContentFilter) => {
    criteria.current = criteria.current.filter((f) => f !== filter);
    rerender();
  };

  return (
    <div>
      {criteria.current.map((filter) => {
        const definition = byId.get(filter.filterDefinitionId);
        return definition ? (
          <FilterEditor
            key={filter.filterDefinitionId}
            definition={definition}
            model={filter}
            mode="search"
            onChange={rerender}
            onDelete={() => remove(filter)}
          />
        ) : null;
      })}
      {available.length > 0 && (
        <select className="form-control mb-2" value="" onChange={(e) => add(e.target.value)}>
          <option value="">+ add criterion…</option>
          {available.map((d) => (
            <option key={d.id} value={d.id}>
              {d.displayName}
            </option>
          ))}
        </select>
      )}
      <button type="button" className="cms-button" onClick={() => onFilter(structuredClone(criteria.current))}>
        <span className="fa fa-search" /> Search
      </button>
    </div>
  );
}
