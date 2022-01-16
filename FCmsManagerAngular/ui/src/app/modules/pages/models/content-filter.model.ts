import { IFilterModel } from '../../../models/filter-model';

export interface IContentFilterModel {
    filterDefinitionId?: string;
    values: string[];
    filterType: string;
}