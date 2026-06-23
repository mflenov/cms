import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
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
  contents: IContentModel[] = [];
  pagesSubs!: Subscription;

  constructor(
    private contentService: ContentService,
    private toastService: ToastService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.pagesSubs = this.contentService.getContents().subscribe((content: IContentModel[]) => {
        this.contents = content;
        this.cdr.detectChanges();
    }, (error: HttpErrorResponse) => {this.toastService.error(error.message, error.status);});
  }

  ngOnDestroy(): void {
    this.pagesSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    this.pagesSubs = this.contentService.deleteById(id!).subscribe({
      next: () => {
        const index = this.contents.findIndex(m => m.id == id);
        this.contents.splice(index, 1);
        this.cdr.detectChanges();
      }
    });
  }  
}
