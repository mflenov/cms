export interface IDbRowModel {
  values: string[];
}

export interface IDbColumnModel {
  name: string;
  isPrimaryKey: Boolean;
}

export interface IDbContentModel {
  columns: IDbColumnModel[];
  rows: IDbRowModel[];
}
