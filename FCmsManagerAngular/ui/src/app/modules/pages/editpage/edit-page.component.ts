import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IContentFilterModel } from '../models/content-filter.model';

import { IPageContentModel } from '../models/page-content.model';
import { IPageStructureModel } from '../models/page-structure.model';
import { ContentService } from '../services/content.service'
import { PagesService } from '../services/pages.service';

@Component({
  selector: 'pg-editpage',
  templateUrl: './edit-page.component.html',
  styleUrls: ['./edit-page.component.css'],
  providers: [ContentService, PagesService]
})

export class EditpageComponent implements OnInit, OnDestroy {
  private id: string = "";
  filters: IContentFilterModel[] = [];

  data: IPageContentModel = {} as IPageContentModel;
  definition: IPageStructureModel = {} as IPageStructureModel;

  constructor(
    private contentService: ContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const idvalue = this.route.snapshot.paramMap.get('id');

    if (idvalue) {
      this.id = idvalue;

      this.contentService.getPageContent(this.id, this.filters).subscribe(content => {
        this.pagesService.getPage(this.id).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.definition = definition.data as IPageStructureModel;
            this.data = (content.data as IPageContentModel);
          }
        })
      })
    }
  }

  ngOnDestroy(): void {
  }

  onSubmit(): void {
    this.contentService.save(this.data).subscribe({
      next: data => {
        this.router.navigate(['/pages']);
      }
    });
  }

  onFilter(filters: IContentFilterModel[]): void {
    this.filters = filters;

    this.ngOnInit();
  }
}
