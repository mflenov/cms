import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { PagesService } from '../../../services/pages.service';
import { IDbContentModel, IDbRowModel } from '../models/dncontent.model';
import { DbContentService } from '../services/dbcontent.service';

@Component({
  selector: 'db-edit-content-component',
  templateUrl: './edit-db-content-component.component.html',
  styleUrls: ['./edit-db-content-component.component.css'],
  providers: [PagesService, DbContentService]
})
export class EditDbContentComponentComponent implements OnInit {
  data: IDbRowModel = {} as IDbRowModel;
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

          this.data.values = this.definition.contentDefinitions.map(x => "");
        }
      })
    }
  }

  onSubmit(): void {
  }
}
