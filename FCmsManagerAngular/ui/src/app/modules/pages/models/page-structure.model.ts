import { IContentDefinitionsModel } from './content-definitions.model'

export interface IPageStructureModel {
  id?: string;
  name: string;
  contentDefinitions: IContentDefinitionsModel[];
}