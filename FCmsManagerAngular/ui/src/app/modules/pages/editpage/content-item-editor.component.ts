import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, Output, EventEmitter } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { ContentPlaceholderDirective } from './content-placeholder.directive';
import { EditFiltersComponent } from './edit-filters.component';

@Component({
  selector: 'pg-content-item-editor',
  templateUrl: './content-item-editor.component.html',
  styleUrls: ['./content-item-editor.component.css']
})
export class ContentItemEditorComponent implements OnInit {
  @Input() definition!: IContentDefinitionsModel;
  @Input() content!: any;
  @Input() folderItemEditor: Boolean = false;

  @Output() onDelete: EventEmitter<string> = new EventEmitter();
  @Output() onAddFolder: EventEmitter<any> = new EventEmitter();
  isFiltersPanelVisible: boolean = false;

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  constructor(private componentFactoryResolver: ComponentFactoryResolver) { }

  ngOnInit(): void {
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
      const newFilderItem = this.getFolderModel(id);
      (this.content as [any]).push(newFilderItem);
      this.onAddFolder.emit(newFilderItem);
    }
  }

  getFolderModel(id: string) : IContentItemModel {
    let model: IContentItemModel = {
      definitionId: id,
      isFolder: true,
      isDeleted: false,
      filters: [],
      data: null,
      children: []
    };
    for (const key in this.definition.contentDefinitions) {
      model.children.push(
        {
          definitionId: this.definition.contentDefinitions[key].definitionId,
          isFolder: false,
          isDeleted: false,
          filters: [],
          data: null,
          children: []
        }
      );
    }
    return model;
  }
}
