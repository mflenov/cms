import { Component, OnInit, OnDestroy, signal } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

import { IContentModel } from '../../../models/content.model';
import { ContentService } from '../../..//services/content.service';

import { ToastService } from '../../..//shared/services/toast.service';

@Component({
  selector: 'app-content-list',
  templateUrl: './content-list.component.html',
  styleUrl: './content-list.component.css',
  providers: [ContentService, ToastService],
  standalone: false
})

export class ContentListComponent implements OnInit, OnDestroy {
  private _contents = signal<IContentModel[]>([]);
  contents = this._contents.asReadonly();
  pagesSubs!: Subscription;

  constructor(
    private contentService: ContentService,
    private toastService: ToastService,
  ) { }

  ngOnInit(): void {
    this.pagesSubs = this.contentService.getContents().subscribe((content: IContentModel[]) => {
        this._contents.set(content);
    }, (error: HttpErrorResponse) => {this.toastService.error(error.message, error.status);});
  }

  ngOnDestroy(): void {
    this.pagesSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    this.pagesSubs = this.contentService.deleteById(id!).subscribe({
      next: () => {
        this._contents.update(contents => contents.filter(m => m.id !== id));
      }
    });
  }  
}
