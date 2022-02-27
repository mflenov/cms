import { Component, OnInit, ComponentFactoryResolver, ViewChild, Output, EventEmitter } from '@angular/core';

import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from '../../../models/filter-model';
import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { IContentFilterModel } from '../models/content-filter.model';
import { FilterControlService } from '../services/filter-control.service';

@Component({
  selector: 'pg-filters',
  templateUrl: './filters.component.html',
  styleUrls: ['./filters.component.css'],
  providers: [FiltersService, FilterControlService]
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
    private componentFactoryResolver: ComponentFactoryResolver,
    private filterControlService: FilterControlService
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
      this.contentFilters.push(this.filterControlService.createFilterEditor(item, this.filterControlService.createModel(item), this.placeholder));
    }
    this.selectedFilter = "";
  }

  search(): void {
    this.onFilter.emit(this.contentFilters);
  }
}
