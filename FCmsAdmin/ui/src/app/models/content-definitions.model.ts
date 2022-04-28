import { IContentFilterModel } from '../modules/pages/models/content-filter.model'

export interface IContentDefinitionsModel {
  definitionId?: string;
  name: string;
  typeName: string;
  contentDefinitions: IContentDefinitionsModel[];
  filter?: IContentFilterModel;
}