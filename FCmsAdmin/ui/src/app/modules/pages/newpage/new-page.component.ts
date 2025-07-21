import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { CmsenumsService } from '../../../services/cmsenums.service';
import { INewPageModel } from '../../../models/new-page.model';
import { PagesService } from '../../../services/pages.service';

@Component({
  selector: 'app-new-page',
  templateUrl: './new-page.component.html',
  styleUrls: ['./new-page.component.css'],
  providers: [PagesService],
  standalone: false
})
export class NewPageComponent implements OnInit {
  model: INewPageModel = {} as INewPageModel;

  templates!: Observable<string[]>;

  constructor(
    private cmsenumsService: CmsenumsService,
    private pagesService: PagesService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const filterTypeSubs = this.cmsenumsService.getEnums().subscribe({
      next: model => {
        this.templates = of(model.pageTemplates);
      }
    });
  }

  onSubmit(form: NgForm): void {
    this.pagesService.create(this.model).subscribe({
      next: data => {
        this.router.navigate(['/pages']);
      }
    });
  }
}
