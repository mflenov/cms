import { Component, OnInit, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { IContentFilterModel } from '../../../models/content-filter.model';
import {NgbDate, NgbCalendar, NgbDateParserFormatter} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-date-filter-editor',
    templateUrl: './date-filter-editor.component.html',
    styleUrls: ['./date-filter-editor.component.css'],
    standalone: false
})
export class DateFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
  @Input() title: string = "";
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  date: NgbDate | null = null;

  constructor(private host: ElementRef<HTMLElement>, private calendar: NgbCalendar, public formatter: NgbDateParserFormatter) { 
  }

  ngOnInit(): void {
    if (this.model.values.length > 0)  {
      this.date = this.getNgbDate(this.model.values[0]);
    }
  }

  delete(): void {
    this.onDelete.emit(this.model.filterDefinitionId);
    this.host.nativeElement.remove();
  }

  getNgbDate(str: string): NgbDate | null {
    if (!str) {
      return null;
    }
    const date = new Date(str);
    return  { day: date.getDate(), month: date.getMonth() + 1, year: date.getFullYear()} as NgbDate;
  }

  getDateString(ngdate: NgbDate | null): string {
    if (ngdate == null) {
      return ""
    }
    let date = new Date(ngdate.year, ngdate.month-1, ngdate.day);
    return date.toISOString();
  }

  onDateSelection(d: NgbDate): void {
    this.model.values[0] = this.getDateString(d);
  }
}
