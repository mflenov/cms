import { Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { IContentDefinitionsModel } from '../../../models/content-definitions.model';
import { IContentItemModel } from '../../../models/content-item.model';
import { IContentListModel } from '../../../models/content-list.model';
import { PageItemService } from '../services/page-item.service';
import { FiltersService } from 'src/app/services/filters.service';
import { IFilterModel } from 'src/app/models/filter-model';
import { IContentFilterModel } from '../../../models/content-filter.model';
import { ContentPlaceholderDirective } from '../../../shared/widgets/content-placeholder.directive';
import { ContentItemComponent } from '../../../shared/editcontent/content-item.component'
import { EditFiltersComponent } from '../../../shared/editcontent/edit-filters.component';

@Component({
    selector: 'app-list-page-content',
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
    private componentFactoryResolver: ComponentFactoryResolver,
    private contentService: PageItemService,
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

  edit(contentid: string | undefined) {
    this.placeholder.viewContainerRef.clear();

    let contentEditorComponent = this.componentFactoryResolver.resolveComponentFactory(ContentItemComponent);
    let contentEditorComponentRef = this.placeholder.viewContainerRef.createComponent(contentEditorComponent);
    (<ContentItemComponent>(contentEditorComponentRef.instance)).definition = this.definition;
    (<ContentItemComponent>(contentEditorComponentRef.instance)).filters = this.filters;
    this.selectedIndex = this.data.findIndex(m => m.id == contentid);
    (<ContentItemComponent>(contentEditorComponentRef.instance)).data = [ this.data[this.selectedIndex] ];
    (<ContentItemComponent>(contentEditorComponentRef.instance)).isControlsVisible = false;

    let filtersComponent = this.componentFactoryResolver.resolveComponentFactory(EditFiltersComponent);
    let filtersComponentRef = this.placeholder.viewContainerRef.createComponent(filtersComponent);
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
