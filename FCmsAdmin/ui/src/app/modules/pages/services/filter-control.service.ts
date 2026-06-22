import { Injectable, EventEmitter, Output } from '@angular/core';

import { IContentFilterModel } from '../../../models/content-filter.model';
import { IFilterModel } from 'src/app/models/filter-model';
import { ContentPlaceholderDirective } from '../../../shared/widgets/content-placeholder.directive';

import { TextFilterEditorComponent } from '../../../shared/editcontent/filter-controls/text-filter-editor.component';
import { BoolFilterEditorComponent } from '../../../shared/editcontent/filter-controls/bool-filter-editor.component';
import { DaterangeFilterEditorComponent } from '../../../shared/editcontent/filter-controls/daterange-filter-editor.component';
import { DateFilterEditorComponent } from '../../../shared/editcontent/filter-controls/date-filter-editor.component';
import { ValuelistFilterEditorComponent } from '../../../shared/editcontent/filter-controls/valuelist-filter-editor.component';
import { RegexFilterEditorComponent } from '../../../shared/editcontent/filter-controls/regex-filter-editor.component';

@Injectable()

export class FilterControlService {
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

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
      (<any>(componentRef.instance)).title = filter.displayName;
      ((<any>(componentRef.instance)).onDelete as EventEmitter<string>).subscribe(item => this.onFilterDelete(item));
    }
    return model;
  }

  onFilterDelete(id: string) {
    this.onDelete.emit(id);
  }

  createEditorComponent(type: string, iseditor: Boolean): any {
    if (type == "Text") {
      return TextFilterEditorComponent;
    }
    if (type == "Boolean") {
      return BoolFilterEditorComponent;
    }
    if (type == "DateRange" && iseditor) {
      return DaterangeFilterEditorComponent;
    }
    if (type == "DateRange" && !iseditor) {
      return DateFilterEditorComponent;
    }
    if (type == "ValueList") {
      return ValuelistFilterEditorComponent;
    }
    if (type == "RegEx") {
      return RegexFilterEditorComponent;
    }
    return null;
  }
}