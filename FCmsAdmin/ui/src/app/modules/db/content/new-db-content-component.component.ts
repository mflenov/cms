import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { PagesService } from '../../../services/pages.service';
import { IDbContentModel } from '../models/dncontent.model';
import { DbContentService } from '../services/dbcontent.service';

@Component({
  selector: 'db-new-content-component',
  templateUrl: './new-db-content-component.component.html',
  styleUrls: ['./new-db-content-component.component.css'],
  providers: [PagesService, DbContentService]
})
export class NewDbContentComponentComponent implements OnInit {
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
      this.pagesService.getPage(this.definitionId).subscribe(definition => {
        if (definition.status == 1 && definition.data) {
          this.definition = definition.data as IPageStructureModel;
        }
      })
    }
  }

  onSubmit(): void {
  }
}
