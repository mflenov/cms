import { IContentItemModel } from './content-item.model'
import { IContentDefinitionsModel } from './content-definitions.model'

export interface IContentListModel  {
  contentItems: IContentItemModel[];
  definition: IContentDefinitionsModel;
  repositoryName: string; 
}