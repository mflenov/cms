import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, of } from 'rxjs';

import { PagesService } from '../services/pages.service';
import { IPageStructureModel } from '../models/page-structure.model'

@Component({
  selector: 'app-content-definition',
  templateUrl: './content-definition.component.html',
  styleUrls: ['./content-definition.component.css'],
  providers: [PagesService]
})

export class ContentDefinitionComponent implements OnInit {
  private modelSubs!: Subscription;

  model: IPageStructureModel = {} as IPageStructureModel;

  constructor(private route: ActivatedRoute, private pagesService: PagesService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
  }
}
