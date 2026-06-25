import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { Subscription } from 'rxjs';

import { FiltersService } from '../../../services/filters.service';
import { IFilterModel } from '../../../models/filter-model';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
    selector: 'app-filters',
    templateUrl: './filter-list.component.html',
    styleUrls: ['./filter-list.component.css'],
    providers: [FiltersService, ToastService],
    standalone: false
})

export class FiltersComponent implements OnInit, OnDestroy {
  private _filters = signal<IFilterModel[]>([]);
  filters = this._filters.asReadonly();
  filtersSubs!: Subscription;

  constructor(
    private filtersService: FiltersService,
    private toastService: ToastService,
  ) {
  }

  ngOnInit(): void {
    this.filtersSubs = this.filtersService.getFilters().subscribe(
      filters => {
        this._filters.set(filters.data as IFilterModel[]);
      }
      , error => {this.toastService.error(error.message, error.status);}
    );
  }

  ngOnDestroy(): void {
    this.filtersSubs.unsubscribe();
  }

  deleteRow(id: string | undefined): void {
    this.filtersService.deleteById(id!).subscribe({
      next: result => {
        this._filters.update(filters => filters.filter(m => m.id !== id));
      }
    });
  }
}
