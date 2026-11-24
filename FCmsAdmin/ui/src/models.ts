// API models, mirroring the view models returned by the FCmsAdmin controllers.

export interface ApiResult<T = any> {
  status: number;
  description: string;
  data: T;
}

export const API_SUCCESS = 1;

export interface Enums {
  filterTypes: string[];
  contentDataTypes: string[];
  pageTemplates: string[];
  databaseTypes: string[];
}

export interface FilterDefinition {
  id?: string;
  name: string;
  displayName: string;
  type: string;
  values: string[];
}

export interface User {
  id?: string;
  username: string;
  password?: string;
}

export interface DbConnection {
  id?: string;
  name: string;
  connectionString: string;
  databaseType: string;
}

/** A repository as listed on the Pages / Content / Database screens. */
export interface RepositorySummary {
  id?: string;
  name: string;
}

export interface NewPage {
  name: string;
  template: string;
}

export interface NewContent {
  name: string;
}

export interface NewDbRepository {
  name: string;
  connectionId: string;
}

export interface ContentFilter {
  filterDefinitionId?: string;
  values: string[];
  filterType: string;
  dataType: string;
}

export interface ContentDefinition {
  definitionId?: string;
  name: string;
  typeName: string;
  contentDefinitions: ContentDefinition[];
  filter?: ContentFilter;
}

export interface RepositoryStructure {
  id?: string;
  name: string;
  contentDefinitions: ContentDefinition[];
}

export interface ContentItem {
  id?: string;
  definitionId: string;
  toolTip?: string;
  data: any;
  isFolder: boolean;
  isDeleted: boolean;
  filters: ContentFilter[];
  children: ContentItem[];
}

export interface RepositoryContent {
  contentItems: ContentItem[];
}

export interface ContentList {
  contentItems: ContentItem[];
  definition: ContentDefinition;
  repositoryName: string;
}

export interface PreviewItem {
  name: string;
  value: string;
  children: PreviewItem[] | null;
}

export interface DbColumn {
  name: string;
  isPrimaryKey: boolean;
}

export interface DbRow {
  values: any[];
}

export interface DbContent {
  columns: DbColumn[];
  rows: DbRow[];
}

/** Which repository section a shared screen (structure, edit, list) belongs to. */
export type Section = 'pages' | 'content' | 'db';
