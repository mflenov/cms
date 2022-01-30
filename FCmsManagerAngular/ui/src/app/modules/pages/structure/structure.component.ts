import { NgForm } from '@angular/forms'
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, of, Observable } from 'rxjs';

import { CmsenumsService } from '../../../services/cmsenums.service'
import { PagesService } from '../services/pages.service'
import { IEnumsModel } from '../../../models/enums-model'
import { IPageStructureModel } from '../models/page-structure.model'

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

  constructor(private route: ActivatedRoute,
    private router: Router,
    private pagesService: PagesService,
    private cmsenumsService: CmsenumsService) { }

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
}
