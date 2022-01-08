export interface IContentDefinitions {
  definitionId?: string;
  name: string;
  typename: string;
}

export interface IPageStructureModel {
  id?: string;
  name: string;
  contentDefinitions: IContentDefinitions[];
}