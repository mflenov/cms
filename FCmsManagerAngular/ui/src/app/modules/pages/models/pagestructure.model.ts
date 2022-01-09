export interface IContentDefinitions {
  definitionId?: string;
  name: string;
  typeName: string;
  contentDefinitions: IContentDefinitions[];
}

export interface IPageStructureModel {
  id?: string;
  name: string;
  contentDefinitions: IContentDefinitions[];
}