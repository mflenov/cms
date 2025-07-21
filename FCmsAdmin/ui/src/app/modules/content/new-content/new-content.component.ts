import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';

import { INewContentModel } from 'src/app/models/new-content.model';
import { ContentService } from 'src/app/services/content.service';

@Component({
  selector: 'app-new-content',
  templateUrl: './new-content.component.html',
  styleUrl: './new-content.component.css',
  providers: [ContentService],
  standalone: false
})
export class NewContentComponent {
  model: INewContentModel = {} as INewContentModel;

  constructor(
    private contentService: ContentService,
    private router: Router
  ) { }


  onSubmit(form: NgForm): void {
    this.contentService.create(this.model).subscribe({
      next: data => {
        this.router.navigate(['/content']);
      }
    });
  }

}
