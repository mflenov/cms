import { IContentDefinitionsModel } from './content-definitions.model'

export interface IContentStructureModel {
  id?: string;
  name: string;
  contentDefinitions: IContentDefinitionsModel[];
}