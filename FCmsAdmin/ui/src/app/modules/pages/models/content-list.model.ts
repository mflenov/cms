import { IContentItemModel } from './content-item.model'
import { IContentDefinitionsModel } from '../../../models/content-definitions.model'

export interface IContentListModel  {
  contentItems: IContentItemModel[];
  definition: IContentDefinitionsModel;
  repositoryName: string; 
}