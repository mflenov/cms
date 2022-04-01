import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouteConfigLoadEnd } from '@angular/router';
import { IContentFilterModel } from '../models/content-filter.model';
import { IPageContentModel } from '../models/page-content.model';
import { IPagePreviewModel } from '../models/page-preview.model';
import { IPageStructureModel } from '../models/page-structure.model';
import { ContentService } from '../services/content.service';
import { PagesService } from '../services/pages.service';

@Component({
  selector: 'app-preview',
  templateUrl: './preview.component.html',
  styleUrls: ['./preview.component.css'],
  providers: [ContentService, PagesService]
})
export class PreviewComponent implements OnInit {
  searchfilters: IContentFilterModel[] = [];
  id: string = "";
  filters: IContentFilterModel[] = [];

  data: IPagePreviewModel[] = [];
  definition: IPageStructureModel = {} as IPageStructureModel;

  constructor(
    private contentService: ContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    ) { }

  ngOnInit(): void {
    const idvalue = this.route.snapshot.paramMap.get('id');

    if (idvalue) {
      this.id = idvalue;

      this.pagesService.getPage(this.id).subscribe(definition => {
        if (definition.status == 1 && definition.data) {
          this.definition = definition.data as IPageStructureModel;
        }
        this.onFilter(this.filters);
      })
    }
  }

  onFilter(filters: IContentFilterModel[]): void {
    this.searchfilters = filters;

    this.contentService.filterPageContent(this.id, this.searchfilters).subscribe(content => {
      this.data = (content.data as IPagePreviewModel[]);
    })
  }
}
