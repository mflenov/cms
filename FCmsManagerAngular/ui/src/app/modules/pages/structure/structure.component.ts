import { Component, ComponentFactoryResolver, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of, Observable } from 'rxjs';

import { CmsenumsService } from '../../../services/cmsenums.service'
import { PagesService } from '../services/pages.service'
import { IPageStructureModel } from '../models/page-structure.model'
import { ContentPlaceholderDirective } from '../editpage/content-placeholder.directive';
import { IContentDefinitionsModel } from '../models/content-definitions.model';

@Component({
  selector: 'stre-structure',
  templateUrl: './structure.component.html',
  styleUrls: ['./structure.component.css'],
  providers: [PagesService]
})

export class StructureComponent implements OnInit, OnDestroy {
  private modelSubs!: Subscription;
  private dataTypesSubs!: Subscription;

  dataTypes!: Observable<string[]>

  model: IPageStructureModel = {} as IPageStructureModel;

  @ViewChild(ContentPlaceholderDirective, { static: true }) placeholder!: ContentPlaceholderDirective;

  constructor(private route: ActivatedRoute,
    private router: Router,
    private pagesService: PagesService,
    private cmsenumsService: CmsenumsService
    ) { }

  ngOnInit(): void {
    this.dataTypesSubs = this.cmsenumsService.getEnums().subscribe({
      next: model => { this.dataTypes = of(model.contentDataTypes) }
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.modelSubs = this.pagesService.getPage(id).subscribe({
        next: result => {
          if (result.status == 1 && result.data) {
            this.model = result.data as IPageStructureModel;
          }
        }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.dataTypesSubs) {
      this.dataTypesSubs.unsubscribe();
    }
    if (this.modelSubs) {
      this.modelSubs.unsubscribe();
    }
  }

  onSubmit(): void {
    this.pagesService.save(this.model).subscribe({
      next: data => {
        this.router.navigate(['/pages']);
      }
    });
  }

  onAddValue(): void {
    const model = {
      name: "",
      typeName: "String",
      contentDefinitions: []
    } as IContentDefinitionsModel;
    this.model.contentDefinitions.push(model);
  }

  deleteRow(id: string | undefined): void {
    debugger;
    const index = this.model.contentDefinitions.findIndex(m => m.definitionId == id);
    this.model.contentDefinitions.splice(index, 1);
  }
}
