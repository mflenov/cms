import { Component, OnDestroy, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IContentFilterModel } from '../../models/content-filter.model';

import { IPageContentModel } from '../../models/page-content.model';
import { IPageStructureModel } from '../../models/page-structure.model';
import { PageContentService } from '../../modules/pages/services/page-content.service'
import { PagesService } from '../../services/pages.service';

import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
    selector: 'sh-editcontent',
    templateUrl: './edit-content.component.html',
    styleUrls: ['./edit-content.component.css'],
    providers: [PageContentService, PagesService, ToastService],
    standalone: false
})

export class EditContentComponent implements OnInit, OnDestroy {
  private id: string = "";
  filters: IContentFilterModel[] = [];

  data: IPageContentModel = {} as IPageContentModel;
  definition: IPageStructureModel = {} as IPageStructureModel;

  constructor(
    private pageContentService: PageContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    const idvalue = this.route.snapshot.paramMap.get('id');

    if (idvalue) {
      this.id = idvalue;

      this.pageContentService.getPageContent(this.id, this.filters).subscribe(content => {
        this.pagesService.getPage(this.id).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.definition = definition.data as IPageStructureModel;
            this.data = (content.data as IPageContentModel);
          }
        }, error => {this.toastService.error(error.message, error.status);})
      }, error => {this.toastService.error(error.message, error.status);})
    }
  }

  ngOnDestroy(): void {
  }

  onSubmit(): void {
    this.pageContentService.save(this.data).subscribe({
      next: data => {
        this.router.navigate(['../../'],  {relativeTo: this.route});
      }
    });
  }

  onFilter(filters: IContentFilterModel[]): void {
    this.filters = filters;

    this.ngOnInit();
  }
}
