import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IDbContentModel, IDbRowModel } from '../models/dncontent.model';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { DbContentService } from '../services/dbcontent.service';
import { PagesService } from '../../../services/pages.service';
import { Subscription } from 'rxjs';

import { ToastService } from 'src/app/shared/services/toast.service';


@Component({
    selector: 'db-content',
    templateUrl: './db-content.component.html',
    styleUrls: ['./db-content.component.css'],
    providers: [DbContentService, PagesService, ToastService],
    standalone: false
})

export class DbContentComponent implements OnInit, OnDestroy {
  data: IDbContentModel = {} as IDbContentModel;
  definitionId!: string;
  hiddenColumns: string[] = ["_modified", "_created"];

  definition: IPageStructureModel = {} as IPageStructureModel;

  private dbContentSubs!: Subscription;
  private pageSubs!: Subscription;

  constructor(
    private contentService: DbContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.definitionId) {
      this.dbContentSubs = this.contentService.getDbContent(this.definitionId).subscribe(dbcontent => {
        this.pageSubs = this.pagesService.getPage(this.definitionId).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.data = (dbcontent.data as IDbContentModel);
            this.definition = definition.data as IPageStructureModel;
          }
          this.pageSubs.unsubscribe();
        }, error => {this.toastService.error(error.message, error.status);})
        this.dbContentSubs.unsubscribe();
      }, error => {this.toastService.error(error.message, error.status);})
    }
  }

  ngOnDestroy(): void {
  }

  getValue(row: IDbRowModel, columnName: string) {
    for (let index = 0; index < this.data.columns.length; index++) {
      if (this.data.columns[index].name == columnName) {
        return row.values[index].toString();
      }
    }
    return  "1";
  }

  deleteItem(id: string): void {
    this.contentService.delete(this.definitionId, id).subscribe({
      next: data => {
        const index = this.data.rows.findIndex(m => m.values[0] == id);
        this.data.rows.splice(index, 1);
      }
    });
  }
}
