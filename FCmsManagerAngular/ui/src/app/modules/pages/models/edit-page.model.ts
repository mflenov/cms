import { IContentDefinitions } from './content-definitions.model'

export interface IContentListViewModel {
    repositoryId?: string;
    repositoryName: string;
    contentDefinitions: IContentDefinitions[];
}