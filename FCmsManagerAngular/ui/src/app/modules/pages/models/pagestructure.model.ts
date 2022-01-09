export interface IContentDefinitions {
  definitionId?: string;
  name: string;
  typeName: string;
}

export interface IPageStructureModel {
  id?: string;
  name: string;
  contentDefinitions: IContentDefinitions[];
}