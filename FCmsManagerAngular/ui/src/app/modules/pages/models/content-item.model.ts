import { IContentFilterModel } from './content-filter.model';

export interface IContentItemModel {
  id?: string;
  definitionId: string;
  toolTip?: string;
  data: any;
  isFolder: boolean;
  filters: IContentFilterModel[];
  children: IContentItemModel[];
}