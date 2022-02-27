import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { IContentFilterModel } from '../models/content-filter.model';
import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';
import { Subscription } from 'rxjs';

import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { FilterControlService } from '../services/filter-control.service';

@Component({
  selector: 'pg-edit-filters',
  templateUrl: './edit-filters.component.html',
  styleUrls: ['./edit-filters.component.css'],
  providers: [FiltersService, FilterControlService]
})
export class EditFiltersComponent implements OnInit {
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
    this.filtersSubs = this.filtersService.getFilters().subscribe({
      next: filters => {
        this.availableFilters = filters.data as IFilterModel[];

        for (const key in filters.data as IFilterModel[]){ 
          let filter = filters.data[key];
          this.filterDefinitions[filter.id] = filter;
        }

        this.isLoaded = false;
        this.nofilters = this.model?.length == 0;
      }
    });
  }

  addfilter(): void {
    const item = this.availableFilters.find(x => x.id == this.selectedFilter);
    if (item) {
      this.availableFilters = this.availableFilters.filter(i => i.id != item.id);
      //this.contentFilters.push(model);
      this.filterControlService.createFilterEditor(item, this.placeholder);
    }
    this.selectedFilter = "";
    this.nofilters = false;
  }
}
