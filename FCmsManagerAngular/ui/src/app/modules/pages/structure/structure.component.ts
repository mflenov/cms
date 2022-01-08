import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, of } from 'rxjs';

import { PagesService } from '../pages.service';
import { IPageStructureModel } from '../models/pagestructure.model'

@Component({
  selector: 'app-structure',
  templateUrl: './structure.component.html',
  styleUrls: ['./structure.component.css'],
  providers: [PagesService]
})

export class StructureComponent implements OnInit {
  private modelSubs!: Subscription;

  model: IPageStructureModel = {} as IPageStructureModel;

  constructor(private route: ActivatedRoute, private pagesService: PagesService) { }

  ngOnInit(): void {
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

  deleteRow(id: string | undefined): void {

  }  
}
