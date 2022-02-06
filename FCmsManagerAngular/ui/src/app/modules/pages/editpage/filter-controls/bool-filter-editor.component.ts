import { Component, OnInit, Input } from '@angular/core';
import { IContentFilterModel } from '../../models/content-filter.model';

@Component({
  selector: 'pg-bool-filter-editor',
  templateUrl: './bool-filter-editor.component.html',
  styleUrls: ['./bool-filter-editor.component.css']
})

export class BoolFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: ['']} as IContentFilterModel;
  @Input() title: string = "";

  constructor() { }

  ngOnInit(): void {
  }
}
