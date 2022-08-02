export interface IDbRowModel {
  values: string[];
}

export interface IDbContentModel {
  columnNames: string[];
  rows: IDbRowModel[];
}
