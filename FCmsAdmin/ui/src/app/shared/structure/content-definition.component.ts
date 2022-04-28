import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { IContentDefinitionsModel } from '../../models/content-definitions.model'
import { Subscription, of, Observable } from 'rxjs';

import { CmsenumsService } from '../../services/cmsenums.service'


@Component({
  selector: 'sh-content-definition',
  templateUrl: './content-definition.component.html',
  styleUrls: ['./content-definition.component.css']
})

export class ContentdefinitionComponent implements OnInit, OnDestroy {
  @Input() definition: IContentDefinitionsModel = {} as IContentDefinitionsModel;

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

  onAddValue(): void {
    const model = {
      name: "",
      typeName: "String",
      contentDefinitions: []
    } as IContentDefinitionsModel;
    this.definition.contentDefinitions.push(model);
  }

  deleteRow(id: string|undefined) {
    if (id) {
      const index = this.definition.contentDefinitions.findIndex(m => m.definitionId == id);
      this.definition.contentDefinitions.splice(index, 1);
    }
  }
}
