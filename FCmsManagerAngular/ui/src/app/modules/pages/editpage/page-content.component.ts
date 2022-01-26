import { Component, OnInit, Input, ViewChild, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { IPageContentModel } from '../models/page-content.model';
import { ContentEditorComponent } from './content-editor.component';
import { ContentPlaceholderDirective } from './content-placeholder.directive';

@Component({
    selector: 'app-page-content',
    templateUrl: './page-content.component.html',
    styleUrls: ['./page-content.component.css']
})
export class PageContentComponent implements OnInit {
    @Input() definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
    @Input() data: IContentItemModel[] = [];

    @ViewChild(ContentPlaceholderDirective, { static: true }) private placeholder!: ContentPlaceholderDirective;

    content: any = {};

    constructor(private componentFactoryResolver: ComponentFactoryResolver,
        private viewContainerRef: ViewContainerRef) { }

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
    }

    createEditModel(definitionId: string): IContentItemModel{
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
        let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(
            ContentEditorComponent
        );
        let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(contentEditorComponent);

        (<ContentEditorComponent>(contentEditorComponentRef.instance)).definition = this.definition;
        (<ContentEditorComponent>(contentEditorComponentRef.instance)).content = model;
    }
}
