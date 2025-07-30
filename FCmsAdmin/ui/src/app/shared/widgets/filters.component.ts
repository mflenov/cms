import { Component, OnInit, ViewChild, Input, Output, EventEmitter } from '@angular/core';

import { FiltersService } from '../../services/filters.service';
import { IFilterModel } from '../../models/filter-model';
import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { IContentFilterModel } from '../../modules/pages/models/content-filter.model';
import { FilterControlService } from '../../modules/pages/services/filter-control.service';

@Component({
    selector: 'sh-filters',
    templateUrl: './filters.component.html',
    styleUrls: ['./filters.component.css'],
    providers: [FiltersService, FilterControlService],
    standalone: false
})

export class FiltersComponent implements OnInit {
  allfilters: IFilterModel[] = [];
  availableFilters: IFilterModel[] = [];
  selectedFilter: string = "";

  contentFilters: IContentFilterModel[] = [];

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  @Output() onFilter: EventEmitter<any> = new EventEmitter();
  @Input() expanded:Boolean = false;

  constructor(
    private filtersService: FiltersService,
    private filterControlService: FilterControlService
  ) {
  }

  ngOnInit(): void {
    this.filterControlService.onDelete.subscribe(item => this.onDeleteFilter(item))

    const filtersSubs = this.filtersService.getCachedFilters().subscribe({
      next: filters => {
        this.allfilters = filters.data as IFilterModel[];
        this.availableFilters = this.allfilters;
        filtersSubs.unsubscribe;
        if (this.expanded) {
          this.expand();
        }
      }
    });
  }

  expand(): void {
    this.allfilters.forEach(item => {
      this.contentFilters.push(this.filterControlService.createFilterControl(item, this.filterControlService.createModel(item), this.placeholder));      
    });
    this.availableFilters = [];
  }

  onDeleteFilter(id: string): void {
    const index = this.contentFilters.findIndex(m => m.filterDefinitionId == id);
    this.contentFilters.splice(index, 1);
    this.availableFilters.push(this.allfilters.find(m => m.id == id)!);
  }

  addFilter(): void {
    const item = this.allfilters.find(x => x.id == this.selectedFilter || x.type == this.selectedFilter);
    if (item) {
      this.availableFilters = this.availableFilters.filter(i => i.id != item.id);
      this.contentFilters.push(this.filterControlService.createFilterControl(item, this.filterControlService.createModel(item), this.placeholder));
    }
    this.selectedFilter = "";
  }

  search(): void {
    this.onFilter.emit(this.contentFilters);
  }
}
