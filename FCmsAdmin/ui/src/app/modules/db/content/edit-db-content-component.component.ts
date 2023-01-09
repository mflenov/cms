import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IPageStructureModel } from '../../../models/page-structure.model';
import { PagesService } from '../../../services/pages.service';
import { IDbRowModel } from '../models/dncontent.model';
import { DbContentService } from '../services/dbcontent.service';

@Component({
  selector: 'db-edit-content-component',
  templateUrl: './edit-db-content-component.component.html',
  styleUrls: ['./edit-db-content-component.component.css'],
  providers: [PagesService, DbContentService]
})
export class EditDbContentComponentComponent implements OnInit {
  data: IDbRowModel = {} as IDbRowModel;
  repositoryId!: string;

  definition: IPageStructureModel = {} as IPageStructureModel;

  constructor(
    private contentService: DbContentService,
    private pagesService: PagesService,
    private route: ActivatedRoute,
    private router: Router
   ) { }

  ngOnInit(): void {
    this.repositoryId = this.route.snapshot.paramMap.get('id') ?? '';

    if (this.repositoryId) {
      this.pagesService.getPage(this.repositoryId).subscribe(definition => {
        if (definition.status == 1 && definition.data) {
          this.definition = definition.data as IPageStructureModel;

          this.data.values = this.definition.contentDefinitions.map(x => "");
        }
      })
    }
  }

  onSubmit(): void {
    this.contentService.save(this.repositoryId, this.data).subscribe({
      next: data => {
        this.router.navigate(['/db/content/' + this.repositoryId]);
      }
    });
  }
}
