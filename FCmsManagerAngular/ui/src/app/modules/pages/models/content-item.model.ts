import { IContentFilterModel } from './contentfilter.model';

export interface IContentItemModel {
  id?: string;
  definitionId?: string;
  toolTip?: string;
  values: string[];
  filters: IContentFilterModel;
}