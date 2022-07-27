import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { IDbContentModel } from '../models/dncontent.model';
import { DbContentService } from '../services/dbcontent.service';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { PagesService } from '../../../services/pages.service';

@Component({
  selector: 'app-content',
  templateUrl: './content.component.html',
  styleUrls: ['./content.component.css'],
  providers: [DbContentService, PagesService]
})
export class ContentComponent implements OnInit, OnDestroy {
  data: IDbContentModel[] = [];
  contentSubs!: Subscription;
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
            this.data = (dbcontent.data as IDbContentModel[]);
          }
        })
      })
    }
  }

  ngOnDestroy(): void {
    this.contentSubs.unsubscribe();
  }
}
