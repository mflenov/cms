import { Injectable } from '@angular/core';
import { IContentDefinitionsModel } from '../../../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';


@Injectable({
  providedIn: 'root'
})

export class ContentItemService {
	getFolderModel(id: string, definition: IContentDefinitionsModel) : IContentItemModel {
		let model: IContentItemModel = {
		  definitionId: id,
		  isFolder: true,
		  isDeleted: false,
		  filters: [],
		  data: null,
		  children: []
		};
		for (const key in definition.contentDefinitions) {
		  model.children.push(
			{
			  definitionId: definition.contentDefinitions[key].definitionId!,
			  isFolder: false,
			  isDeleted: false,
			  filters: [],
			  data: null,
			  children: []
			}
		  );
		}
		return model;
	  }
}