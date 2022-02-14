import { Component, OnInit, Input } from '@angular/core';
import { IContentFilterModel } from '../../models/content-filter.model';

@Component({
  selector: 'app-daterange-filter-editor',
  templateUrl: './daterange-filter-editor.component.html',
  styleUrls: ['./daterange-filter-editor.component.css']
})
export class DaterangeFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
  @Input() title: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
