import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IDbContentModel } from '../models/dncontent.model';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { DbContentService } from '../services/dbcontent.service';
import { PagesService } from '../../../services/pages.service';


@Component({
  selector: 'db-content',
  templateUrl: './db-content.component.html',
  styleUrls: ['./db-content.component.css'],
  providers: [DbContentService, PagesService]
})

export class DbContentComponent implements OnInit, OnDestroy {
  data: IDbContentModel = {} as IDbContentModel;
  definitionId!: string;

  definition: IPageStructureModel = {} as IPageStructureModel;

  constructor(
    private contentService: DbContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.definitionId) {
      this.contentService.getDbContent(this.definitionId).subscribe(dbcontent => {
        this.pagesService.getPage(this.definitionId).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.data = (dbcontent.data as IDbContentModel);
            this.definition = definition.data as IPageStructureModel;
          }
        })
      })
    }
  }

  ngOnDestroy(): void {
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
