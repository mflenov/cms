import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

import { IContentDefinitionsModel } from '../../../models/content-definitions.model';
import { ContentItemService } from '../services/content-item.service';
import { ContentPlaceholderDirective } from '../../../shared/widgets/content-placeholder.directive';
import { EditFiltersComponent } from './edit-filters.component';

@Component({
    selector: 'pg-content-item-editor',
    templateUrl: './content-item-editor.component.html',
    styleUrls: ['./content-item-editor.component.css'],
    standalone: false
})
export class ContentItemEditorComponent implements OnInit {
  @Input() definition!: IContentDefinitionsModel;
  @Input() content!: any;
  @Input() folderItemEditor: Boolean = false;
  @Input() isControlsVisible: boolean = true;

  @Output() onDelete: EventEmitter<string> = new EventEmitter();
  @Output() onAddFolder: EventEmitter<any> = new EventEmitter();

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  isFiltersPanelVisible: boolean = false;
  repositoryId: string = '';
  date: NgbDate | null = null;

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
      private contentItemService: ContentItemService,
      private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.route.snapshot.paramMap.get("id")) {
      this.repositoryId = this.route.snapshot.paramMap.get("id")!;
    }

    if (this.definition.typeName == 'DateTime' || this.definition.typeName == 'Date') {
      this.date = this.getNgbDate(this.content.data);
    }
  }

  showFilters(filters: any): void {
    this.createFiltersComponent(filters);
    this.isFiltersPanelVisible = true;
  }

  onCancelFilters(): void {
    this.isFiltersPanelVisible = false;
  }

  onSaveFilters() {
    this.isFiltersPanelVisible = false;
  }

  createFiltersComponent(filters: any) {
    this.placeholder.viewContainerRef.clear();
    let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(EditFiltersComponent);
    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(contentEditorComponent);
    (<EditFiltersComponent>(contentEditorComponentRef.instance)).model = filters;
  }

  deleteFolderItem(id: string | undefined): void {
    const index = (this.content as [any]).findIndex(m => m.id == id);
    (this.content as [any]).splice(index, 1);
    this.onDelete.emit(id);
  }

  addFolderValue(id: string| undefined): void {
    if (id) {
      const newFilderItem = this.contentItemService.getFolderModel(id, this.definition);
      (this.content as [any]).push(newFilderItem);
      this.onAddFolder.emit(newFilderItem);
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
      this.content.data = null;
      return;
    }
    let date = new Date(d.year, d.month - 1, d.day);
    this.content.data = date.toISOString();
  }
}
