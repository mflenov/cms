// Config section: users, database connections and filter definitions.
import { useEffect, useState, type FormEvent, type ReactNode } from 'react';
import { Link, useNavigate, useParams } from 'react-router';
import { dbConnectionsApi, enumsApi, filtersApi, usersApi } from '../api/services';
import { toast } from '../components/toast';
import { useLoad } from '../hooks';
import type { DbConnection, FilterDefinition, User } from '../models';

export function ConfigHomePage() {
  return (
    <>
      <h1>Config</h1>
      <div className="cms-repository">
        <p>
          <Link to="/config/users" data-cy="userslist-link">
            Users
          </Link>{' '}
          - manage users who can access the CMS
        </p>
        <p>
          <Link to="/config/dbconnections" data-cy="dbconnectionslist-link">
            Database Connections
          </Link>{' '}
          - allow you to add/configure database connections for database stored content
        </p>
        <p>
          <Link to="/config/filters" data-cy="filterslist-link">
            Filters
          </Link>{' '}
          - allow you to add/configure variables to create personalized content
        </p>
      </div>
    </>
  );
}

interface Column<T> {
  header: string;
  width?: string;
  cell: (row: T) => ReactNode;
}

function ConfigTable<T extends { id?: string }>({
  title,
  load,
  remove,
  addUrl,
  columns,
}: {
  title: string;
  load: () => Promise<T[]>;
  remove: (id: string) => Promise<unknown>;
  addUrl: string;
  columns: Column<T>[];
}) {
  const navigate = useNavigate();
  const { data: rows, setData } = useLoad(load, []);

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
      <div className="cms-repository">
        <table className="cms-table">
          <tbody>
            <tr>
              {columns.map((c) => (
                <th key={c.header} style={c.width ? { width: c.width } : undefined}>
                  {c.header}
                </th>
              ))}
              <th></th>
            </tr>
            {rows?.map((row) => (
              <tr id={row.id} key={row.id}>
                {columns.map((c) => (
                  <td key={c.header}>{c.cell(row)}</td>
                ))}
                <td>
                  <span className="fa fa-trash link" title="Delete" onClick={() => deleteRow(row.id!)} />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <br />
        <button type="button" className="cms-button" data-cy="add-button" onClick={() => navigate(addUrl)}>
          Add
        </button>
      </div>
    </>
  );
}

export function FilterListPage() {
  return (
    <ConfigTable<FilterDefinition>
      title="Filters"
      load={filtersApi.list}
      remove={filtersApi.remove}
      addUrl="/config/filter"
      columns={[
        { header: 'Name', width: '40%', cell: (f) => <Link to={`/config/filter/${f.id}`}>{f.name}</Link> },
        { header: 'Display Name', width: '40%', cell: (f) => f.displayName },
        { header: 'Type', cell: (f) => f.type },
      ]}
    />
  );
}

export function DbConnectionListPage() {
  return (
    <ConfigTable<DbConnection>
      title="Database Connections"
      load={dbConnectionsApi.list}
      remove={dbConnectionsApi.remove}
      addUrl="/config/dbconnection"
      columns={[
        { header: 'Name', width: '40%', cell: (c) => <Link to={`/config/dbconnection/${c.id}`}>{c.name}</Link> },
        { header: 'Connection String', width: '40%', cell: (c) => c.connectionString },
        { header: 'Type', cell: (c) => c.databaseType },
      ]}
    />
  );
}

export function UserListPage() {
  return (
    <ConfigTable<User>
      title="Users"
      load={usersApi.list}
      remove={usersApi.remove}
      addUrl="/config/user"
      columns={[{ header: 'Name', width: '40%', cell: (u) => <Link to={`/config/user/${u.id}`}>{u.username}</Link> }]}
    />
  );
}

/**
 * Form state for an edit screen: starts from `initial`, replaced by the loaded record when the
 * route has an :id. `save` posts the model and returns to `backUrl`.
 */
function useEditForm<T>(initial: T, load: (id: string) => Promise<T>, save: (model: T) => Promise<unknown>, backUrl: string) {
  const { id } = useParams();
  const navigate = useNavigate();
  const [model, setModel] = useState<T>(initial);

  useEffect(() => {
    if (!id) return;
    let active = true;
    load(id)
      .then((m) => active && setModel(m))
      .catch((e) => active && toast.fromError(e));
    return () => {
      active = false;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [id]);

  const set = <K extends keyof T>(key: K, value: T[K]) => setModel((m) => ({ ...m, [key]: value }));

  const onSubmit = async (e: FormEvent) => {
    e.preventDefault();
    try {
      await save(model);
      navigate(backUrl);
    } catch (err) {
      toast.fromError(err);
    }
  };

  return { model, setModel, set, onSubmit };
}

export function FilterEditPage() {
  const { data: enums } = useLoad(() => enumsApi.get(), []);
  const { model, set, onSubmit } = useEditForm<FilterDefinition>(
    { name: '', displayName: '', type: 'Text', values: [] },
    filtersApi.get,
    filtersApi.save,
    '/config/filters',
  );
  const values = model.values ?? [];

  return (
    <>
      <h1>Edit filter</h1>
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input id="name" className="form-control" value={model.name} onChange={(e) => set('name', e.target.value)} />
          </div>
          <div className="form-group">
            <label htmlFor="displayname">Display Name</label>
            <input
              id="displayname"
              className="form-control"
              value={model.displayName}
              onChange={(e) => set('displayName', e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="filterType">Type</label>
            <select id="filterType" className="form-control" value={model.type} onChange={(e) => set('type', e.target.value)}>
              {(enums?.filterTypes ?? [model.type]).map((t) => (
                <option key={t} value={t}>
                  {t}
                </option>
              ))}
            </select>
          </div>
          {model.type === 'ValueList' && (
            <div className="form-group">
              <h3>Values:</h3>
              {values.map((value, i) => (
                <div className="form-group row" key={i}>
                  <div className="col-sm-11">
                    <input
                      className="form-control"
                      value={value}
                      autoComplete="off"
                      autoCapitalize="off"
                      placeholder="Value"
                      onChange={(e) => set('values', values.map((v, j) => (j === i ? e.target.value : v)))}
                    />
                  </div>
                  <div
                    className="col-sm-1 fa fa-trash link"
                    title="Delete value"
                    onClick={() => set('values', values.filter((_, j) => j !== i))}
                  />
                </div>
              ))}
              <div className="cms-right-controls cms-buttons">
                <button type="button" className="cms-button" onClick={() => set('values', [...values, ''])}>
                  Add new value
                </button>
              </div>
            </div>
          )}
          <button className="cms-button">Save</button>
        </form>
      </div>
    </>
  );
}

export function DbConnectionEditPage() {
  const { data: enums } = useLoad(() => enumsApi.get(), []);
  const { model, set, onSubmit } = useEditForm<DbConnection>(
    { name: '', connectionString: '', databaseType: 'PostgresSQL' },
    dbConnectionsApi.get,
    dbConnectionsApi.save,
    '/config/dbconnections',
  );

  return (
    <>
      <h1>Edit Connection</h1>
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input id="name" className="form-control" value={model.name} onChange={(e) => set('name', e.target.value)} />
          </div>
          <div className="form-group">
            <label htmlFor="connectionString">Connection String</label>
            <input
              id="connectionString"
              className="form-control"
              value={model.connectionString}
              onChange={(e) => set('connectionString', e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="databaseType">Connection Type</label>
            <select
              id="databaseType"
              className="form-control"
              value={model.databaseType}
              onChange={(e) => set('databaseType', e.target.value)}
            >
              {(enums?.databaseTypes ?? [model.databaseType]).map((t) => (
                <option key={t} value={t}>
                  {t}
                </option>
              ))}
            </select>
          </div>
          <button className="cms-button">Save</button>
        </form>
      </div>
    </>
  );
}

export function UserEditPage() {
  const { model, set, onSubmit } = useEditForm<User>(
    { username: '', password: '' },
    usersApi.get,
    usersApi.save,
    '/config/users',
  );

  return (
    <>
      <h1>Edit User</h1>
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="username">Username</label>
            <input
              id="username"
              className="form-control"
              value={model.username}
              onChange={(e) => set('username', e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              type="password"
              className="form-control"
              value={model.password ?? ''}
              onChange={(e) => set('password', e.target.value)}
            />
          </div>
          <button className="cms-button">Save</button>
        </form>
      </div>
    </>
  );
}
