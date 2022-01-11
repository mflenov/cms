import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { IPageModel } from '../models/page.model';
import { PagesService } from '../services/pages.service';

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
    this.pagesSubs = this.pagesService.deleteById(id!).subscribe({
      next: data => {
        const index = this.pages.findIndex(m => m.id == id);
        this.pages.splice(index, 1);
      }
    });
  }  
}
