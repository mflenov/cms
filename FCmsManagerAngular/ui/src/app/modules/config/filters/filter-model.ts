import { IApiRequest } from '../../../models/api-request-model'


export interface IFilterModelData {
  id? : string;
  name: string;
  type: string;
}


export interface IFilterModel extends IApiRequest {
  data: IFilterModelData[];
}