import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CmsenumsService } from '../../../services/cmsenums.service';
import { INewDbRepoModel } from '../models/new-dbrepo.model';
import { DbRepoService } from '../services/dbrepo.service';

@Component({
  selector: 'app-new-repo',
  templateUrl: './new-repo.component.html',
  styleUrls: ['./new-repo.component.css'],
  providers: [DbRepoService]
})
export class NewRepoComponent implements OnInit {
  model: INewDbRepoModel = {} as INewDbRepoModel;

  constructor(
    private cmsenumsService: CmsenumsService,
    private repoService: DbRepoService,
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
