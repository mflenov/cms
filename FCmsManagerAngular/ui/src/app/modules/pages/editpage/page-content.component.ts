import { Component, OnInit, Input } from '@angular/core';
import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { IPageContentModel } from '../models/page-content.model';

@Component({
    selector: 'app-page-content',
    templateUrl: './page-content.component.html',
    styleUrls: ['./page-content.component.css']
})
export class PageContentComponent implements OnInit {
    @Input() definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
    content: any = {};

    @Input() data: IContentItemModel[] = [];

    constructor() { }

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
}
