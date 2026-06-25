import { Component, OnInit, OnDestroy, signal } from '@angular/core';
import { Subscription } from 'rxjs';

import { IPageModel } from '../../../models/page.model';
import { PagesService } from '../../../services/pages.service';

import { ToastService } from '../../..//shared/services/toast.service';

@Component({
  selector: 'app-page-list',
  templateUrl: './page-list.component.html',
  styleUrls: ['./page-list.component.css'],
  providers: [PagesService, ToastService],
  standalone: false
})

export class PageListComponent implements OnInit, OnDestroy {
  private _pages = signal<IPageModel[]>([]);
  pages = this._pages.asReadonly();
  pagesSubs!: Subscription;

  constructor(
    private pagesService: PagesService,
    private toastService: ToastService,
  ) { }

  ngOnInit(): void {
    this.pagesSubs = this.pagesService.getPages().subscribe(pages => {
        this._pages.set(pages);
    }, error => {this.toastService.error(error.message, error.status);});
  }

  ngOnDestroy(): void {
    this.pagesSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    this.pagesSubs = this.pagesService.deleteById(id!).subscribe({
      next: () => {
        this._pages.update(pages => pages.filter(m => m.id !== id));
      }
    });
  }  
}
