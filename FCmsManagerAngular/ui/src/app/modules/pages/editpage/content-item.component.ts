import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentFilterModel } from '../models/content-filter.model';
import { IContentItemModel } from '../models/content-item.model';
import { ContentItemService } from '../services/contentitem.service';
import { ContentItemEditorComponent } from './content-item-editor.component';
import { ContentPlaceholderDirective } from './content-placeholder.directive';


@Component({
  selector: 'pg-content-item',
  templateUrl: './content-item.component.html',
  styleUrls: ['./content-item.component.css']
})

export class ContentItemComponent implements OnInit {
  @Input() definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
  @Input() data: IContentItemModel[] = [];
  @Input() folderItem: Boolean = false;
  @Input() filters: IContentFilterModel[] = [];

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  content: any = {};
  isNewItemVisible: boolean = true;

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private viewContainerRef: ViewContainerRef,
    private contentItemService: ContentItemService
  ) { }

  ngOnInit(): void {
    if (this.data && this.data.length > 0) {
      this.buildContentMap();
    }
  }

  buildContentMap() {
    for (const item in this.data) {
      const definitionId = this.data[item].definitionId
      if (this.data[item].isFolder) {
        if (!this.content[definitionId]) {
          this.content[definitionId] = [];
        }
        this.content[definitionId].push(this.data[item]);
      }
      else {
        this.content[definitionId] = this.data[item];
      }
    }
  }

  addValue(definitionId: string) {
    if (this.definition.typeName == 'Folder') {
      const model = this.contentItemService.getFolderModel(definitionId, this.definition);
      model.filters = this.filters;
      this.data.push(model);
      this.createContentComponent([model]);
    }
    else {
      const model = this.createEditModel(definitionId);
      this.data.push(model);
      this.createContentComponent(model);
    }
    this.isNewItemVisible = false;  
  }

  createEditModel(definitionId: string): IContentItemModel {
    const newitem = {} as IContentItemModel;
    newitem.definitionId = definitionId;
    newitem.filters = this.filters;
    newitem.isFolder = false;
    if (this.definition.typeName == 'String') {
      newitem.data = 'new string';
    }
    return newitem;
  }

  createContentComponent(model: any) {
    this.placeholder.viewContainerRef.clear();
    let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(ContentItemEditorComponent);
    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(ContentItemEditorComponent);

    (<ContentItemEditorComponent>(contentEditorComponentRef.instance)).content = model;
    (<ContentItemEditorComponent>(contentEditorComponentRef.instance)).definition = this.definition;
    (<ContentItemEditorComponent>(contentEditorComponentRef.instance)).onAddFolder.subscribe(this.onAddFolder);
    (<ContentItemEditorComponent>(contentEditorComponentRef.instance)).onDelete.subscribe(this.onDelete);
  }

  onDelete(id: string): void {
    const index = this.data.findIndex(m => m.id == id);
    this.data[index].isDeleted = true;
  }

  onAddFolder(folder: any): void {
    this.data.push(folder);
  }
}
