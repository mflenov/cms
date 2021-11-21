import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { IPageModel } from '../models/pagemodel';
import { PagesService } from '../pages.service';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.css'],
  providers: [ PagesService ]
})

export class PageListComponent implements OnInit, OnDestroy {
  pages: IPageModel[] = [];
  pagesSubs!: Subscription;

  constructor(private pagesService: PagesService) { }

  ngOnInit(): void {
    this.pagesSubs = this.pagesService.getPages().subscribe({
      next: pages => {
        this.pages = pages;
      }
    });
  }

  ngOnDestroy(): void {
    this.pagesSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    alert(id);
  }  
}
