import { Component, OnInit, Input } from '@angular/core';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

import { IContentDefinitionsModel } from '../../../models/content-definitions.model';

@Component({
    selector: 'db-content-editor',
    templateUrl: './db-content-editor.component.html',
    styleUrls: ['./db-content-editor.component.css'],
    standalone: false
})
export class DbContentEditorComponent implements OnInit {
  @Input() definition!: IContentDefinitionsModel;
  @Input() content!: any;
  @Input() index!: number;

  date: NgbDate | null = null;

  constructor() { }

  ngOnInit(): void {
    if (this.definition.typeName == 'DateTime' || this.definition.typeName == 'Date') {
      this.date = this.getNgbDate(this.content);
    }
  }

  getNgbDate(str: string): NgbDate | null {
    if (!str) {
      return null;
    }
    const date = new Date(str);
    return { day: date.getDate(), month: date.getMonth() + 1, year: date.getFullYear() } as NgbDate;
  }

  onDateSelection(d: NgbDate): void {
    if (d == null) {
      this.content = null;
      return;
    }
    let date = new Date(d.year, d.month - 1, d.day);
    this.content = date.toISOString();
  }
}
