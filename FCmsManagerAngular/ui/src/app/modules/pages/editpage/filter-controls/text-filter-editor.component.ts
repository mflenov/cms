import { Component, OnInit, Input } from '@angular/core';
import { IContentFilterModel } from '../../models/content-filter.model';

@Component({
  selector: 'pg-text-filter-editor',
  templateUrl: './text-filter-editor.component.html',
  styleUrls: ['./text-filter-editor.component.css']
})
export class TextFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
  @Input() title: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
