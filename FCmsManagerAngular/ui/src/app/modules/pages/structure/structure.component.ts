import { NgForm } from '@angular/forms'
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription, of, Observable } from 'rxjs';

import { PagesService } from '../pages.service';
//import { cmsenumsService } from '../'
import { IPageStructureModel } from '../models/pagestructure.model'

@Component({
  selector: 'app-structure',
  templateUrl: './structure.component.html',
  styleUrls: ['./structure.component.css'],
  providers: [PagesService]
})

export class StructureComponent implements OnInit {
  private modelSubs!: Subscription;
  dataTypes!: Observable<string[]>

  model: IPageStructureModel = {} as IPageStructureModel;

  constructor(private route: ActivatedRoute, private pagesService: PagesService,
     ) { }

  ngOnInit(): void {
    //this.dataTypes = this.

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

  onSubmit(): void {
    debugger;
  }
}
