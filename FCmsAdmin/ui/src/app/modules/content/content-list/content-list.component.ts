import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { IContentModel } from 'src/app/models/content.model';
import { ContentService } from 'src/app/services/content.service';

import { ToastService } from 'src/app/shared/services/toast.service';

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
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.pagesSubs = this.contentService.getContents().subscribe(content => {
        this.contents = content;
    }, error => {this.toastService.error(error.message, error.status);});
  }

  ngOnDestroy(): void {
    this.pagesSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    this.pagesSubs = this.contentService.deleteById(id!).subscribe({
      next: data => {
        const index = this.contents.findIndex(m => m.id == id);
        this.contents.splice(index, 1);
      }
    });
  }  
}
