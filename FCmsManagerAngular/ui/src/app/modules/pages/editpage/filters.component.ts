import { Component, OnInit, ComponentFactoryResolver, ViewChild, Output, EventEmitter } from '@angular/core';

import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from '../../../models/filter-model';
import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { TextFilterEditorComponent } from './filter-controls/text-filter-editor.component'
import { IContentFilterModel } from '../models/content-filter.model';
import { BoolFilterEditorComponent } from './filter-controls/bool-filter-editor.component';

@Component({
  selector: 'pg-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  providers: [FiltersService]
})

export class FiltersComponent implements OnInit {
  allfilters: IFilterModel[] = [];
  availableFilters: IFilterModel[] = [];
  selectedFilter: string = "";

  contentFilters: IContentFilterModel[] = [];

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  @Output() onFilter: EventEmitter<any> = new EventEmitter();

  constructor(
    private filtersService: FiltersService,
    private componentFactoryResolver: ComponentFactoryResolver
  ) {
  }

  ngOnInit(): void {
    const filtersSubs = this.filtersService.getFilters().subscribe({
      next: filters => {
        this.allfilters = filters.data as IFilterModel[];
        this.availableFilters = this.allfilters;
        filtersSubs.unsubscribe;
      }
    });
  }

  addFilter(): void {
    const item = this.allfilters.find(x => x.id == this.selectedFilter);
    if (item) {
      this.availableFilters = this.availableFilters.filter(i => i.id != item.id);
      this.createFilterEditor(item);
    }
  }

  createFilterEditor(filter: IFilterModel): void {
    const model = {
      filterDefinitionId: filter.id,
      filterType: "Include",
      dataType: filter.type,
      values: [""]
    } as IContentFilterModel;

    let component = null as any;
    if (filter.type == "Text") {
      component = this.componentFactoryResolver.resolveComponentFactory(TextFilterEditorComponent);
    }

    if (filter.type == "Boolean") {
      component = this.componentFactoryResolver.resolveComponentFactory(BoolFilterEditorComponent);
    }

    if (component) {
      let componentRef = this.placeholder.viewContainerRef.createComponent(component);
      (<any>(componentRef.instance)).model = model;
      (<any>(componentRef.instance)).title = filter.name;
      this.contentFilters.push(model);
    }
  }

  search(): void {
    this.onFilter.emit(this.contentFilters);
  }
}
