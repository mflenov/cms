import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { IPageModel } from '../../../models/page.model';
import { PagesService } from '../../../services/pages.service';

import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.css'],
  providers: [PagesService, ToastService],
  standalone: false
})

export class PageListComponent implements OnInit, OnDestroy {
  pages: IPageModel[] = [];
  pagesSubs!: Subscription;

  constructor(
    private pagesService: PagesService,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.pagesSubs = this.pagesService.getPages().subscribe(pages => {
        this.pages = pages;
    }, error => {this.toastService.error(error.message, error.status);});
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
