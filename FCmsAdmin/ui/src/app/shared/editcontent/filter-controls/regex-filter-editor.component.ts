import { Component, OnInit, Input, Output, EventEmitter, ElementRef } from '@angular/core';
import { IContentFilterModel } from '../../../modules/pages/models/content-filter.model';

@Component({
    selector: 'sh-regex-filter-editor',
    templateUrl: './regex-filter-editor.component.html',
    styleUrls: ['./regex-filter-editor.component.css'],
    standalone: false
})
export class RegexFilterEditorComponent implements OnInit {
  @Input() model: IContentFilterModel = { values: [''] } as IContentFilterModel;
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
