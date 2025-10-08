import { IFilterModel } from './filter-model';

export interface IContentFilterModel {
  filterDefinitionId?: string;

  values: string[];

  filterType: string;

  dataType: string;
}