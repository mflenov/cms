import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from '../../../models/filter-model';

@Component({
  selector: 'app-filters',
  templateUrl: './filter-list.component.html',
  styleUrls: ['./filter-list.component.css'],
  providers: [FiltersService]
})

export class FiltersComponent implements OnInit, OnDestroy {
  filters: IFilterModel[] = [];
  filtersSubs!: Subscription;

  constructor(private filtersService: FiltersService) {
  }

  ngOnInit(): void {
    this.filtersSubs = this.filtersService.getFilters().subscribe({
      next: filters => {
        this.filters = filters.data as IFilterModel[];
      }
    });
  }

  ngOnDestroy(): void {
    this.filtersSubs.unsubscribe();
  }

  deleteRow(id: string | undefined): void {
    this.filtersService.deleteById(id!).subscribe({
      next: result => {
        const index = this.filters.findIndex(m => m.id == id);
        this.filters.splice(index, 1);
      }
    });
  }
}
