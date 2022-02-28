import { Component, OnInit, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { IContentFilterModel } from '../../models/content-filter.model';

@Component({
  selector: 'pg-bool-filter-editor',
  templateUrl: './bool-filter-editor.component.html',
  styleUrls: ['./bool-filter-editor.component.css']
})

export class BoolFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: ['']} as IContentFilterModel;
  @Input() title: string = "";
  @Output() onDelete: EventEmitter<string> = new EventEmitter();

  constructor(private host: ElementRef<HTMLElement>) { }

  ngOnInit(): void {
  }

  delete(): void {
    this.onDelete.emit(this.model.filterDefinitionId);
    this.host.nativeElement.remove();
  }
}
