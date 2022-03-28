import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IContentFilterModel } from '../models/content-filter.model';
import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';
import { Subscription } from 'rxjs';

import { ContentPlaceholderDirective } from '../widgets/content-placeholder.directive';
import { FilterControlService } from '../services/filter-control.service';

@Component({
  selector: 'pg-edit-filters',
  templateUrl: './edit-filters.component.html',
  styleUrls: ['./edit-filters.component.css'],
  providers: [FiltersService, FilterControlService]
})
export class EditFiltersComponent implements OnInit, OnDestroy {
  @Input() model: IContentFilterModel[] = [];

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  availableFilters: IFilterModel[] = [];
  filterDefinitions: any = [];
  filtersSubs!: Subscription;
  isLoaded: boolean = true;
  selectedFilter: string = "";
  nofilters: boolean = true;

  constructor(private filtersService: FiltersService,
      private filterControlService: FilterControlService
    ) { }

  ngOnInit(): void {
    this.filterControlService.onDelete.subscribe(item => this.onDeleteFilter(item))

    this.filtersSubs = this.filtersService.getCachedFilters().subscribe({
      next: filters => {
        this.availableFilters = filters.data as IFilterModel[];

        for (const key in filters.data as IFilterModel[]){ 
          let filter = filters.data[key];
          this.filterDefinitions[filter.id] = filter;
        }

        for (const key in this.model){ 
          const item = this.availableFilters.find(x => x.id == this.model[key].filterDefinitionId);
          if (item) {
            this.availableFilters = this.availableFilters.filter(i => i.id != item.id);
            this.filterControlService.createFilterEditor(item, this.model[key], this.placeholder);
          }      
        }

        this.isLoaded = false;
        this.nofilters = this.model?.length == 0;
      }
    });
  }

  ngOnDestroy(): void {
    if (this.filtersSubs) {
      this.filtersSubs.unsubscribe();
    }
  }

  onDeleteFilter(id: string): void {
    const index = this.model.findIndex(m => m.filterDefinitionId == id);
    this.model.splice(index, 1);
  }

  addfilter(): void {
    const item = this.availableFilters.find(x => x.id == this.selectedFilter);
    if (item) {
      this.availableFilters = this.availableFilters.filter(i => i.id != item.id);
      this.model.push(this.filterControlService.createFilterEditor(item, this.filterControlService.createModel(item), this.placeholder));
    }
    this.selectedFilter = "";
    this.nofilters = false;
  }
}
