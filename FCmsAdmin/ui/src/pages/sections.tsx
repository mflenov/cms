// Pages, Content and Database sections: repository lists and "new repository" forms.
import { useState, type FormEvent } from 'react';
import { useNavigate } from 'react-router';
import { contentApi, dbConnectionsApi, dbRepositoriesApi, enumsApi, pagesApi } from '../api/services';
import { toast } from '../components/toast';
import { useLoad } from '../hooks';
import { PageHeader } from '../components/ui';
import { RepositoryList } from './repository';

export function PageListPage() {
  return (
    <RepositoryList
      title="Pages"
      load={pagesApi.list}
      remove={pagesApi.remove}
      addUrl="/pages/add"
      links={(id) => [
        ['Edit page', `/pages/edit/${id}`],
        ['Page structure', `/pages/structure/${id}`],
        ['Preview', `/pages/preview/${id}`],
      ]}
    />
  );
}

export function ContentListPage() {
  return (
    <RepositoryList
      title="Content"
      load={contentApi.list}
      remove={contentApi.remove}
      addUrl="/content/add"
      links={(id) => [
        ['Edit content', `/content/edit/${id}`],
        ['Content structure', `/content/structure/${id}`],
        ['Preview', `/content/preview/${id}`],
      ]}
    />
  );
}

export function DbRepositoryListPage() {
  return (
    <RepositoryList
      title="DB repositories"
      load={dbRepositoriesApi.list}
      remove={dbRepositoriesApi.remove}
      addUrl="/db/add"
      links={(id) => [
        ['Edit content', `/db/content/${id}`],
        ['Repository structure', `/db/structure/${id}`],
      ]}
    />
  );
}

/** Submits `save()` and navigates to `backUrl` on success. */
function useSubmit(save: () => Promise<unknown>, backUrl: string) {
  const navigate = useNavigate();
  return async (e: FormEvent) => {
    e.preventDefault();
    try {
      await save();
      navigate(backUrl);
    } catch (err) {
      toast.fromError(err);
    }
  };
}

export function NewPagePage() {
  const [name, setName] = useState('');
  const [template, setTemplate] = useState('');
  const { data: enums } = useLoad(() => enumsApi.get(), []);
  const templates = enums?.pageTemplates ?? [];
  const onSubmit = useSubmit(() => pagesApi.create({ name, template: template || templates[0] || '' }), '/pages');

  return (
    <>
      <PageHeader title="New page" back={{ to: '/pages', label: 'Back to list' }} />
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              id="name"
              className="form-control"
              required
              autoFocus
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="template">Type</label>
            <select
              id="template"
              className="form-control"
              value={template}
              onChange={(e) => setTemplate(e.target.value)}
            >
              {templates.map((t) => (
                <option key={t} value={t}>
                  {t}
                </option>
              ))}
            </select>
          </div>
          <div className="cms-buttons">
            <button className="cms-button">Save</button>
          </div>
        </form>
      </div>
    </>
  );
}

export function NewContentPage() {
  const [name, setName] = useState('');
  const onSubmit = useSubmit(() => contentApi.create({ name }), '/content');

  return (
    <>
      <PageHeader title="New content" back={{ to: '/content', label: 'Back to list' }} />
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              id="name"
              className="form-control"
              required
              autoFocus
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>
          <div className="cms-buttons">
            <button className="cms-button">Save</button>
          </div>
        </form>
      </div>
    </>
  );
}

export function NewDbRepositoryPage() {
  const [name, setName] = useState('');
  const [connectionId, setConnectionId] = useState('');
  const { data: connections } = useLoad(() => dbConnectionsApi.list(), []);
  const onSubmit = useSubmit(
    () => dbRepositoriesApi.create({ name, connectionId: connectionId || connections?.[0]?.id || '' }),
    '/db',
  );

  return (
    <>
      <PageHeader title="New repository" back={{ to: '/db', label: 'Back to list' }} />
      <div className="cms-repository">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <label htmlFor="name">Name</label>
            <input
              id="name"
              className="form-control"
              required
              autoFocus
              value={name}
              onChange={(e) => setName(e.target.value)}
            />
          </div>
          <div className="form-group">
            <label htmlFor="connection">Connection</label>
            <select
              id="connection"
              className="form-control"
              value={connectionId}
              onChange={(e) => setConnectionId(e.target.value)}
            >
              {connections?.map((c) => (
                <option key={c.id} value={c.id}>
                  {c.name}
                </option>
              ))}
            </select>
          </div>
          <div className="cms-buttons">
            <button className="cms-button">Save</button>
          </div>
        </form>
      </div>
    </>
  );
}
