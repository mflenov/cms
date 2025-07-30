import { Component, OnInit, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { Subscription } from 'rxjs';
import { IFilterModel } from 'src/app/models/filter-model';
import { FiltersService } from 'src/app/services/filters.service';
import { IContentFilterModel } from '../../../modules/pages/models/content-filter.model';

@Component({
    selector: 'app-valuelist-filter-editor',
    templateUrl: './valuelist-filter-editor.component.html',
    styleUrls: ['./valuelist-filter-editor.component.css'],
    standalone: false
})
export class ValuelistFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
  @Input() title: string = "";
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  filtersSubs!: Subscription;
  values: string[] = []

  constructor(
    private host: ElementRef<HTMLElement>,
    private filtersService: FiltersService) { }

  ngOnInit(): void {
    this.filtersSubs = this.filtersService.getCachedFilters().subscribe({
      next: filters => {
        for (const key in filters.data as IFilterModel[]){ 
          if (filters.data[key].id == this.model.filterDefinitionId) {
            this.values = filters.data[key].values;
            break;
          }
        }
      }
    });
  }

  ngOnDestroy(): void {
    if (this.filtersSubs) {
      this.filtersSubs.unsubscribe();
    }
  }

  delete(): void {
    this.onDelete.emit(this.model.filterDefinitionId);
    this.host.nativeElement.remove();
  }
}
