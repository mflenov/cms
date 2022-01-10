import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { IContentDefinitions } from '../models/pagestructure.model'
import { Subscription, of, Observable } from 'rxjs';

import { CmsenumsService } from '../../../services/cmsenums.service'
import { IEnumsModel } from '../../../models/enums-model'


@Component({
  selector: 'app-contentdefinition',
  templateUrl: './contentdefinition.component.html',
  styleUrls: ['./contentdefinition.component.css']
})
export class ContentdefinitionComponent implements OnInit, OnDestroy {
  @Input() content: IContentDefinitions = {} as IContentDefinitions;

  dataTypes!: Observable<string[]>

  private dataTypesSubs!: Subscription;

  constructor(private cmsenumsService: CmsenumsService) { }

  ngOnInit(): void {
    this.dataTypesSubs = this.cmsenumsService.getEnums().subscribe({
      next: model => { this.dataTypes = of(model.contentDataTypes) }
    });
  }

  ngOnDestroy(): void {
    if (this.dataTypesSubs) {
      this.dataTypesSubs.unsubscribe();
    }
  }

  deleteRow(id: string | undefined): void {

  }
}
