import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { IContentItemModel } from '../models/content-item.model';

import { IPageContentModel } from '../models/page-content.model';
import { IPageStructureModel } from '../models/page-structure.model';
import { ContentService } from '../services/content.service'
import { PagesService } from '../services/pages.service';
import { PageContentComponent } from './page-content.component'

@Component({
  selector: 'app-editpage',
  templateUrl: './edit-page.component.html',
  styleUrls: ['./edit-page.component.css'],
  providers: [ContentService, PagesService]
})

export class EditpageComponent implements OnInit, OnDestroy {
  data: IPageContentModel = {} as IPageContentModel;
  definition: IPageStructureModel = {} as IPageStructureModel;

  private contentSubs!: Subscription;
  private definitionSubs!: Subscription;

  constructor(
    private contentService: ContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (id) {
      this.contentService.getPageContent(id).subscribe(content => {
        this.pagesService.getPage(id).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.definition = definition.data as IPageStructureModel;

            this.data = (content.data as IPageContentModel);
            for (const item in this.data.contentItems) {
              const definitionId = this.data.contentItems[item].definitionId;
            }
          }
        })
      })
    }
  }

  ngOnDestroy(): void {
    if (this.contentSubs) {
      this.contentSubs.unsubscribe();
    }
    if (this.definitionSubs) {
      this.definitionSubs.unsubscribe();
    }
  }

  onSubmit(): void {
    this.contentService.save(this.data).subscribe({
      next: data => {
        this.router.navigate(['/pages']);
      }
    });
  }
}
