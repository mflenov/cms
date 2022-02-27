import { Component, Input, OnInit } from '@angular/core';
import { IContentFilterModel } from '../models/content-filter.model';
import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'pg-edit-filters',
  templateUrl: './edit-filters.component.html',
  styleUrls: ['./edit-filters.component.css'],
  providers: [FiltersService]
})
export class EditFiltersComponent implements OnInit {
  @Input() model: IContentFilterModel[] = [];
  filterDefinitions: any = [];
  filtersSubs!: Subscription;
  isLoaded: boolean = true;

  constructor(private filtersService: FiltersService) { }

  ngOnInit(): void {
    this.filtersSubs = this.filtersService.getFilters().subscribe({
      next: filters => {
        for (const key in filters.data as IFilterModel[]){ 
          let filter = filters.data[key];
          this.filterDefinitions[filter.id] = filter;
        }
        this.isLoaded = false;
      }
    });
  }
}
