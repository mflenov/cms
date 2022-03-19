import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { ContentItemService } from '../services/content-item.service';
import { ContentPlaceholderDirective } from '../widgets/content-placeholder.directive';
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

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  isFiltersPanelVisible: boolean = false;
  repositoryId: string = '';

  constructor(private componentFactoryResolver: ComponentFactoryResolver,
      private contentItemService: ContentItemService,
      private route: ActivatedRoute) { }

  ngOnInit(): void {
    if (this.route.snapshot.paramMap.get("id")) {
      this.repositoryId = this.route.snapshot.paramMap.get("id")!;
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
}
