import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { FiltersService } from './filters.service';
import { IFilterModel } from './filter-model';

@Component({
  selector: 'app-filters',
  templateUrl: './filter-list.component.html',
  styleUrls: ['./filter-list.component.css'],
  providers: [ FiltersService ]
})

export class FiltersComponent implements OnInit, OnDestroy {
  filters: IFilterModel[] = [];
  filtersSubs!: Subscription;

  constructor(private filtersService: FiltersService) {
  }

  ngOnInit(): void {
    this.filtersSubs = this.filtersService.getFilters().subscribe({
      next: filters => {
        this.filters = filters;
      }
    });
  }

  ngOnDestroy(): void {
    this.filtersSubs.unsubscribe();
  }

  deleteRow(id: string): void {
    const index = this.filters.findIndex(m => m.id == id);
    this.filters.splice(index, 1);
  }
}