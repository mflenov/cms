import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { IContentDefinitionsModel } from '../models/content-definitions.model';
import { IContentItemModel } from '../models/content-item.model';
import { IContentListModel } from '../models/content-list.model';
import { ContentService } from '../services/content.service';
import { FiltersService } from 'src/app/services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';

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
  filters: any = {};

  filtersSubs!: Subscription;
  contentSubs!: Subscription;

  constructor(
    private contentService: ContentService,
    private route: ActivatedRoute,
    private filtersService: FiltersService
  ) { }

  ngOnInit(): void {
    const repositoryId = this.route.snapshot.paramMap.get('repo');
    const definitionId = this.route.snapshot.paramMap.get('id');

    if (repositoryId && definitionId) {
      this.filtersSubs = this.filtersService.getFilters().subscribe(filters => {
        this.contentSubs = this.contentService.listPageContent(repositoryId, definitionId).subscribe(content => {
          this.data = (content.data as IContentListModel).contentItems;
          this.definition = (content.data as IContentListModel).definition;
          this.repositoryName = (content.data as IContentListModel).repositoryName;

          const foltersList = filters.data as IFilterModel[];
          for (const key in foltersList) {
            this.filters[foltersList[key].id!] = foltersList[key];
          }
        })
      });
    }
  }

  ngOnDestroy(): void {
    this.filtersSubs.unsubscribe();
    this.contentSubs.unsubscribe();
  }
}
