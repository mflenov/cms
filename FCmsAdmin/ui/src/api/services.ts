import { http } from './http';
import { setToken } from './auth';
import type {
  ApiResult,
  ContentFilter,
  ContentItem,
  DbConnection,
  DbRow,
  Enums,
  FilterDefinition,
  NewContent,
  NewDbRepository,
  NewPage,
  RepositoryContent,
  RepositoryStructure,
  RepositorySummary,
  User,
} from '../models';

export const authApi = {
  async login(username: string, password: string): Promise<void> {
    const result = await http.post<ApiResult<{ token?: string }>>('v1/auth/login', { username, password });
    if (!result?.data?.token) throw new Error(result?.description || 'Login failed.');
    setToken(result.data.token);
  },
};

let enumsCache: Promise<Enums> | null = null;
export const enumsApi = {
  /** Enums never change at runtime, so they are fetched once per page load. */
  get(): Promise<Enums> {
    enumsCache ??= http.get<Enums>('v1/data/enums').catch((e) => {
      enumsCache = null;
      throw e;
    });
    return enumsCache;
  },
};

/** CRUD for the config screens, which share the list/item URL shape and PUT=create, PATCH=update. */
function configApi<T extends { id?: string }>(listPath: string, itemPath: string) {
  return {
    list: () => http.get<ApiResult<T[]>>(listPath).then((r) => r.data ?? []),
    get: (id: string) => http.get<T>(`${itemPath}/${id}`),
    remove: (id: string) => http.delete<ApiResult>(`${itemPath}/${id}`),
    save: (model: T) => (model.id ? http.patch(itemPath, model) : http.put(itemPath, model)),
  };
}

export const filtersApi = configApi<FilterDefinition>('v1/config/filters', 'v1/config/filter');
export const dbConnectionsApi = configApi<DbConnection>('v1/config/dbconnections', 'v1/config/dbconnection');
const usersBase = configApi<User>('v1/config/users', 'v1/config/user');
export const usersApi = {
  ...usersBase,
  // The API has no GET config/user/{id}; look the user up in the list instead.
  get: async (id: string): Promise<User> => {
    const user = (await usersBase.list()).find((u) => u.id === id);
    if (!user) throw new Error(`User ${id} not found`);
    return user;
  },
};

/** Repository structure (definitions) is shared by pages, content and DB repositories. */
export const structureApi = {
  get: (id: string) => http.get<ApiResult<RepositoryStructure>>('v1/repository/structure/' + id),
  save: (model: RepositoryStructure) => http.patch('v1/repository/Page', model),
};

export const pagesApi = {
  list: () => http.get<RepositorySummary[]>('v1/repositories/Page'),
  create: (model: NewPage) => http.put('v1/repository/Page', model),
  remove: (id: string) => http.delete<ApiResult>('v1/repository/Page/' + id),
};

export const contentApi = {
  list: () => http.get<RepositorySummary[]>('v1/repositories/Content'),
  create: (model: NewContent) => http.put('v1/repository/Content', model),
  remove: (id: string) => http.delete<ApiResult>('v1/repository/Content/' + id),
};

export const repositoryItemsApi = {
  getContent: (repositoryid: string, filters: ContentFilter[]) =>
    http.post<ApiResult<RepositoryContent>>('v1/content', { repositoryid, filters }),
  filterContent: (repositoryid: string, filters: ContentFilter[]) =>
    http.post<ApiResult>('v1/content/filter', { repositoryid, filters }),
  listContent: (repositoryId: string, definitionId: string, filters: ContentFilter[] = []) =>
    http.post<ApiResult>(`v1/content/list/${repositoryId}/${definitionId}`, { filters }),
  save: (model: RepositoryContent) => http.put('v1/content', model),
  remove: (repositoryId: string, contentId: string) => http.delete<ApiResult>(`v1/content/${repositoryId}/${contentId}`),
  updateItem: (repositoryId: string, model: ContentItem) => http.put('v1/contentitem/' + repositoryId, model),
};

export const dbRepositoriesApi = {
  list: () => http.get<RepositorySummary[]>('v1/db'),
  create: (model: NewDbRepository) => http.put('v1/db', model),
  remove: (id: string) => http.delete<ApiResult>('v1/db/' + id),
};

export const dbContentApi = {
  list: (repositoryid: string) => http.post<ApiResult>('v1/dbcontent', { repositoryid }),
  save: (repositoryId: string, row: DbRow) => http.put('v1/dbcontent', { repositoryId, row }),
  remove: (repositoryId: string, id: string) =>
    http.delete<ApiResult>(
      `v1/dbcontent?repositoryid=${encodeURIComponent(repositoryId)}&id=${encodeURIComponent(id)}`,
    ),
};
