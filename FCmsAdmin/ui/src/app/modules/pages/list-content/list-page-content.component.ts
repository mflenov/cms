import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { IContentListModel } from '../models/content-list.model';
import { ContentService } from '../services/content.service';
import { FiltersService } from 'src/app/services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';
import { IContentFilterModel } from '../models/content-filter.model';

@Component({
  selector: 'app-list-page-content',
  templateUrl: './list-page-content.component.html',
  styleUrls: ['./list-page-content.component.css'],
  providers: [FiltersService]
})
export class ListPageContentComponent implements OnInit, OnDestroy {
  repositoryName: string = '';
  data: IContentItemModel[] = {} as IContentItemModel[];
  definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
  searchfilters: IContentFilterModel[] = [];
  filters: any = {};

  filtersSubs!: Subscription;
  contentSubs!: Subscription;

  repositoryId: string = '';
  definitionId: string = '';

  constructor(
    private contentService: ContentService,
    private route: ActivatedRoute,
    private filtersService: FiltersService
  ) { }

  ngOnInit(): void {
    this.repositoryId = this.route.snapshot.paramMap.get('repo') ?? '';
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.repositoryId && this.definitionId) {
      this.filtersSubs = this.filtersService.getFilters().subscribe(filters => {
        this.contentSubs = this.contentService.listPageContent(this.repositoryId, this.definitionId, this.searchfilters).subscribe(content => {
          this.data = (content.data as IContentListModel).contentItems;
          this.definition = (content.data as IContentListModel).definition;
          this.repositoryName = (content.data as IContentListModel).repositoryName;

          const foltersList = filters.data as IFilterModel[];
          for (const key in foltersList) {
            this.filters[foltersList[key].id!] = foltersList[key];
          }
          this.filtersSubs.unsubscribe();
          this.contentSubs.unsubscribe();
        })
      });
    }
  }

  ngOnDestroy(): void {
    this.filtersSubs.unsubscribe();
    this.contentSubs.unsubscribe();
  }

  onFilter(filters: IContentFilterModel[]): void {
    this.searchfilters = filters;
    this.ngOnInit();
  }

  delete(contentid: string | undefined) {
    if (contentid) {
      this.contentSubs = this.contentService.deleteById(this.repositoryId, contentid).subscribe({
        next: data => {
          debugger;
          const index = this.data.findIndex(m => m.id == contentid);
          this.data.splice(index, 1);
        }
      });
    }
  }

  edit(id: string | undefined) {

  }
}
