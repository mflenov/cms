import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IDbContentModel } from '../models/dncontent.model';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { DbContentService } from '../services/dbcontent.service';
import { PagesService } from '../../../services/pages.service';


@Component({
  selector: 'app-dbcontent',
  templateUrl: './dbcontent.component.html',
  styleUrls: ['./dbcontent.component.css'],
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
  )
  { }

  ngOnInit(): void {
    this.definitionId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.definitionId) {
      this.contentService.getDbContent(this.definitionId).subscribe(dbcontent => {
        this.pagesService.getPage(this.definitionId).subscribe(definition => {
          if (definition.status == 1 && definition.data) {
            this.definition = definition.data as IPageStructureModel;
            this.data = (dbcontent.data as IDbContentModel);
          }
        })
      })
    }
  }

  ngOnDestroy(): void {
  }
}
