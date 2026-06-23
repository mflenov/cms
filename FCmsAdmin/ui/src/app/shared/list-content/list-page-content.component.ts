import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { IContentDefinitionsModel } from '../../models/content-definitions.model';
import { IContentItemModel } from '../../models/content-item.model';
import { IContentListModel } from '../../models/content-list.model';
import { RepositoryItemService } from '../../services/repository-item.service';
import { FiltersService } from '../../services/filters.service';
import { IFilterModel } from '../..//models/filter-model';
import { IContentFilterModel } from '../../models/content-filter.model';
import { ContentPlaceholderDirective } from '../widgets/content-placeholder.directive';
import { ContentItemComponent } from '../editcontent/content-item.component'
import { EditFiltersComponent } from '../editcontent/edit-filters.component';

@Component({
    selector: 'sh-list-page-content',
    templateUrl: './list-page-content.component.html',
    styleUrls: ['./list-page-content.component.css'],
    providers: [FiltersService],
    standalone: false
})
export class ListPageContentComponent implements OnInit, OnDestroy {
  repositoryName: string = '';
  data: IContentItemModel[] = [];
  definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;
  searchfilters: IContentFilterModel[] = [];
  filters: any = {};

  filtersSubs!: Subscription;
  contentSubs!: Subscription;

  repositoryId: string = '';
  definitionId: string = '';
  isContentEditorVisible: boolean = false;
  selectedIndex: number = -1;

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  constructor(
    private contentService: RepositoryItemService,
    private route: ActivatedRoute,
    private filtersService: FiltersService
  ) { }

  ngOnInit(): void {
    this.repositoryId = this.route.snapshot.paramMap.get('repo') ?? '';
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.repositoryId && this.definitionId) {
      this.filtersSubs = this.filtersService.getFilters().subscribe(filters => {
        this.contentSubs = this.contentService.listContent(this.repositoryId, this.definitionId, this.searchfilters).subscribe(content => {
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

  edit(contentid: string | undefined) {
    this.placeholder.viewContainerRef.clear();

    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(ContentItemComponent);
    (<ContentItemComponent>(contentEditorComponentRef.instance)).definition = this.definition;
    (<ContentItemComponent>(contentEditorComponentRef.instance)).filters = this.filters;
    this.selectedIndex = this.data.findIndex(m => m.id == contentid);
    (<ContentItemComponent>(contentEditorComponentRef.instance)).data = [ this.data[this.selectedIndex] ];
    (<ContentItemComponent>(contentEditorComponentRef.instance)).isControlsVisible = false;

    let filtersComponentRef = this.placeholder.viewContainerRef.createComponent(EditFiltersComponent);
    (<EditFiltersComponent>(filtersComponentRef.instance)).model = this.data[this.selectedIndex].filters;

    (<ContentItemComponent>(contentEditorComponentRef.instance)).isNewItemVisible = false;


    this.isContentEditorVisible = true;
  }

  onSaveContentChanges() {
    if (this.selectedIndex >= 0) {
      this.contentSubs = this.contentService.updateItemById(this.repositoryId, this.data[this.selectedIndex]).subscribe();
    }
    this.isContentEditorVisible = false;
  }

  onCancelContentChanges() {
    this.isContentEditorVisible = false;    
  }
}
