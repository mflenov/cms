import { IContentFilterModel } from './content-filter.model'

export interface IContentDefinitionsModel {
  definitionId?: string;
  name: string;
  typeName: string;
  contentDefinitions: IContentDefinitionsModel[];
  filter?: IContentFilterModel;
}