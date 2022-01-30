import { Component, OnInit, Input } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';

@Component({
  selector: 'pg-content-editor',
  templateUrl: './content-editor.component.html',
  styleUrls: ['./content-editor.component.css']
})
export class ContentEditorComponent implements OnInit {
  @Input() definition!: IContentDefinitionsModel;
  @Input() content!: any;

  constructor() { }

  ngOnInit(): void {
  }
}
