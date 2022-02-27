import { ComponentFactoryResolver, Injectable } from '@angular/core';

import { IContentFilterModel } from '../models/content-filter.model';
import { IFilterModel } from 'src/app/models/filter-model';
import { ContentPlaceholderDirective } from '../editpage/content-placeholder.directive';

import { TextFilterEditorComponent } from '../editpage/filter-controls/text-filter-editor.component';
import { BoolFilterEditorComponent } from '../editpage/filter-controls/bool-filter-editor.component';
import { DaterangeFilterEditorComponent } from '../editpage/filter-controls/daterange-filter-editor.component';

@Injectable()

export class FilterControlService {
	constructor(
		private componentFactoryResolver: ComponentFactoryResolver
	) 
  { }

  createFilterEditor(filter: IFilterModel, placeholder?: ContentPlaceholderDirective): IContentFilterModel {
    const model = {
      filterDefinitionId: filter.id,
      filterType: "Include",
      dataType: filter.type,
      values: [""]
    } as IContentFilterModel;

    let component = this.createComponent(filter.type);

    if (component && placeholder) {
      let componentRef = placeholder.viewContainerRef.createComponent(component);
      (<any>(componentRef.instance)).model = model;
      (<any>(componentRef.instance)).title = filter.name;
    }
    return model;
  }

  createComponent(type: string): any {
    if (type == "Text") {
      return this.componentFactoryResolver.resolveComponentFactory(TextFilterEditorComponent);
    }

    if (type == "Boolean") {
      return this.componentFactoryResolver.resolveComponentFactory(BoolFilterEditorComponent);
    }

    if (type == "DateRange") {
      return this.componentFactoryResolver.resolveComponentFactory(DaterangeFilterEditorComponent);
    }
    return null;
  }
}