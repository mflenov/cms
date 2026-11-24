// Database-backed repositories: row list and "add row" form.
import { useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router';
import { dbContentApi, structureApi } from '../api/services';
import { RichTextEditor } from '../components/inputs';
import { toast } from '../components/toast';
import { keyOf, useLoad } from '../hooks';
import { API_SUCCESS, type DbContent, type DbRow } from '../models';

const HIDDEN_COLUMNS = ['_modified', '_created'];

export function DbContentPage() {
  const { id = '' } = useParams();
  const { data, setData } = useLoad(async () => {
    const [content, structure] = await Promise.all([dbContentApi.list(id), structureApi.get(id)]);
    if (structure.status !== API_SUCCESS || !structure.data) return undefined;
    return { rows: content.data as DbContent, name: structure.data.name };
  }, [id]);

  const columnValue = (row: DbRow, columnName: string) => {
    const index = data?.rows.columns.findIndex((c) => c.name === columnName) ?? -1;
    return index >= 0 ? String(row.values[index] ?? '') : '';
  };

  const deleteRow = async (rowId: string) => {
    try {
      await dbContentApi.remove(id, rowId);
      setData((d) => d && { ...d, rows: { ...d.rows, rows: d.rows.rows.filter((r) => r.values[0] !== rowId) } });
    } catch (e) {
      toast.fromError(e);
    }
  };

  return (
    <>
      <h1>{data?.name}</h1>
      <div className="row">
        <div className="col-md-8">
          {data?.rows?.rows?.map((row) => (
            <div className="cms-repository-item" key={String(row.values[0])}>
              <div className="cms-right fa fa-times link" title="Delete" onClick={() => deleteRow(row.values[0])} />
              {data.rows.columns.map((column, i) =>
                i > 0 && !HIDDEN_COLUMNS.includes(column.name) ? (
                  <div className="cms-db-row-value" key={column.name}>
                    <label>{column.name}</label>
                    <div>{String(row.values[i] ?? '')}</div>
                  </div>
                ) : null,
              )}
              <div className="cms-db-row-footer">
                <span>Created: {columnValue(row, '_created')}</span>
                <span>Modified: {columnValue(row, '_modified')}</span>
              </div>
            </div>
          ))}
        </div>
        <div className="col-md-4">
          <Link to={`/db/content/add/${id}`} className="cms-button">
            Add content
          </Link>
        </div>
      </div>
    </>
  );
}

export function AddDbContentPage() {
  const { id = '' } = useParams();
  const navigate = useNavigate();
  const { data: structure } = useLoad(async () => {
    const result = await structureApi.get(id);
    return result.status === API_SUCCESS ? result.data : undefined;
  }, [id]);
  const [values, setValues] = useState<Record<number, string>>({});

  if (!structure) return null;
  const definitions = structure.contentDefinitions ?? [];

  const setValue = (index: number, value: string) => setValues((v) => ({ ...v, [index]: value }));

  const save = async () => {
    try {
      await dbContentApi.save(id, { values: definitions.map((_, i) => values[i] ?? '') });
      navigate(`/db/content/${id}`);
    } catch (e) {
      toast.fromError(e);
    }
  };

  return (
    <>
      <h1>"{structure.name}" content</h1>
      <div className="row">
        <div className="col-md-8">
          {definitions.map((definition, i) => (
            <div id={definition.definitionId} className="cms-repository" key={definition.definitionId ?? keyOf(definition)}>
              <h3>{definition.name}</h3>
              {definition.typeName === 'String' && (
                <input className="form-control" value={values[i] ?? ''} onChange={(e) => setValue(i, e.target.value)} />
              )}
              {definition.typeName === 'LongString' && (
                <RichTextEditor value={values[i]} onChange={(html) => setValue(i, html)} />
              )}
            </div>
          ))}
          <button type="button" className="cms-button" onClick={save}>
            Save
          </button>
        </div>
      </div>
    </>
  );
}
