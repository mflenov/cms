import { ComponentFactoryResolver, Injectable, EventEmitter, Output } from '@angular/core';

import { IContentFilterModel } from '../models/content-filter.model';
import { IFilterModel } from 'src/app/models/filter-model';
import { ContentPlaceholderDirective } from '../editpage/content-placeholder.directive';

import { TextFilterEditorComponent } from '../editpage/filter-controls/text-filter-editor.component';
import { BoolFilterEditorComponent } from '../editpage/filter-controls/bool-filter-editor.component';
import { DaterangeFilterEditorComponent } from '../editpage/filter-controls/daterange-filter-editor.component';
import { DateFilterEditorComponent } from '../editpage/filter-controls/date-filter-editor.component';

@Injectable()

export class FilterControlService {
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  constructor(
		private componentFactoryResolver: ComponentFactoryResolver
	) 
  { }

  createModel(filter: IFilterModel) : IContentFilterModel {
    const model = {
      filterDefinitionId: filter.id,
      filterType: "Include",
      dataType: filter.type,
      values: [""]
    } as IContentFilterModel;
    return model;
  }

  createFilterControl(filter: IFilterModel, model: IContentFilterModel, placeholder?: ContentPlaceholderDirective): IContentFilterModel {
    let component = this.createEditorComponent(filter.type, false);
    return this.placeControl(component, filter, model, placeholder);
  }

  createFilterEditor(filter: IFilterModel, model: IContentFilterModel, placeholder?: ContentPlaceholderDirective): IContentFilterModel {
    let component = this.createEditorComponent(filter.type, true);
    return this.placeControl(component, filter, model, placeholder);
  }

  placeControl(component: any, filter: IFilterModel, model: IContentFilterModel, placeholder?: ContentPlaceholderDirective) {
    if (component && placeholder) {
      let componentRef = placeholder.viewContainerRef.createComponent(component);
      (<any>(componentRef.instance)).model = model;
      (<any>(componentRef.instance)).title = filter.name;
      ((<any>(componentRef.instance)).onDelete as EventEmitter<string>).subscribe(item => this.onFilterDelete(item));
    }
    return model;
  }

  onFilterDelete(id: string) {
    this.onDelete.emit(id);
  }

  createEditorComponent(type: string, iseditor: Boolean): any {
    if (type == "Text") {
      return this.componentFactoryResolver.resolveComponentFactory(TextFilterEditorComponent);
    }
    if (type == "Boolean") {
      return this.componentFactoryResolver.resolveComponentFactory(BoolFilterEditorComponent);
    }
    if (type == "DateRange" && iseditor) {
      return this.componentFactoryResolver.resolveComponentFactory(DaterangeFilterEditorComponent);
    }
    if (type == "DateRange" && !iseditor) {
      return this.componentFactoryResolver.resolveComponentFactory(DateFilterEditorComponent);
    }
    return null;
  }
}