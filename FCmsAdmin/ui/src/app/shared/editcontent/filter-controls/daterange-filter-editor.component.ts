import { Component, OnInit, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { IContentFilterModel } from '../../../models/content-filter.model';
import {NgbDate, NgbCalendar, NgbDateParserFormatter} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'sh-daterange-filter-editor',
    templateUrl: './daterange-filter-editor.component.html',
    styleUrls: ['./daterange-filter-editor.component.css'],
    standalone: false
})
export class DaterangeFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
  @Input() title: string = "";
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  hoveredDate: NgbDate | null = null;

  fromDate: NgbDate | null = null;
  toDate: NgbDate | null = null;

  constructor(private host: ElementRef<HTMLElement>, private calendar: NgbCalendar, public formatter: NgbDateParserFormatter) { 
  }

  ngOnInit(): void {
    if (this.model.values.length > 0)  {
      this.fromDate = this.getNgbDate(this.model.values[0]);
    }

    if (this.model.values.length > 1)  {
      this.toDate = this.getNgbDate(this.model.values[1]);
    }
  }

  delete(): void {
    this.onDelete.emit(this.model.filterDefinitionId);
    this.host.nativeElement.remove();
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
    } else if (this.fromDate && !this.toDate && date && date.after(this.fromDate)) {
      this.toDate = date;
    } else {
      this.toDate = null;
      this.fromDate = date;
    }
    this.model.values[0] = this.getDateString(this.fromDate);
    this.model.values[1] = this.getDateString(this.toDate);
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

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.formatter.parse(input);
    const valid = parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
    return valid;
  }
}
