import { IFilterModel } from '../../../models/filter-model';

export interface IContentFilterModel {
  filterDefinitionId?: string;
  contentFilterType: string;
  filter: IFilterModel;
  index: number;
  values: string[];
  filterType: string;
}