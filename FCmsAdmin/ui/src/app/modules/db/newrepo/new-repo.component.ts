import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { CmsenumsService } from '../../../services/cmsenums.service';
import { INewRepoModel } from '../models/new-repo.model';
import { RepoService } from '../services/repo.service';

@Component({
  selector: 'app-new-repo',
  templateUrl: './new-repo.component.html',
  styleUrls: ['./new-repo.component.css'],
  providers: [RepoService]
})
export class NewRepoComponent implements OnInit {
  model: INewRepoModel = {} as INewRepoModel;

  constructor(
    private cmsenumsService: CmsenumsService,
    private repoService: RepoService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm): void {
    this.repoService.create(this.model).subscribe({
      next: data => {
        this.router.navigate(['/db']);
      }
    });
  }
}
