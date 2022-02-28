import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, ViewContainerRef, ElementRef } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { ContentEditorComponent } from './content-editor.component';
import { ContentPlaceholderDirective } from './content-placeholder.directive';


@Component({
  selector: 'pg-content-item',
  templateUrl: './content-item.component.html',
  styleUrls: ['./content-item.component.css']
})

export class ContentItemComponent implements OnInit {
  @Input() definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
  @Input() data: IContentItemModel[] = [];

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  content: any = {};
  isNewItemVisible: boolean = true;

  constructor(
    private componentFactoryResolver: ComponentFactoryResolver,
    private viewContainerRef: ViewContainerRef
  ) { }

  ngOnInit(): void {
    if (this.data && this.data.length > 0) {
      this.buildContentMap();
    }
  }

  deleteRow(id: string | undefined): void {
    debugger;
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
    const model = this.createEditModel(definitionId);
    this.createContentComponent(model);
    this.isNewItemVisible = false;
  }

  createEditModel(definitionId: string): IContentItemModel {
    const newitem = {} as IContentItemModel;
    newitem.definitionId = definitionId;

    if (this.definition.typeName == 'String') {
      newitem.isFolder = false;
      newitem.data = 'new string';
    }
    this.data.push(newitem);
    return newitem;
  }

  createContentComponent(model: IContentItemModel) {
    this.placeholder.viewContainerRef.clear();
    let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(ContentEditorComponent);
    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(contentEditorComponent);

    (<ContentEditorComponent>(contentEditorComponentRef.instance)).definition = this.definition;
    (<ContentEditorComponent>(contentEditorComponentRef.instance)).content = model;
  }
}
