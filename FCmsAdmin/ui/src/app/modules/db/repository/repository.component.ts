import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IDbRepositoryModel } from '../models/dbrepository.model';
import { DbRepoService } from '../services/dbrepo.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['./repository.component.css'],
  providers: [DbRepoService, ToastService]
})

export class RepositoryComponent implements OnInit, OnDestroy {
  repositories: IDbRepositoryModel[] = [];
  repoSubs!: Subscription;

  constructor(private repoService: DbRepoService, private toastService: ToastService) { }

  ngOnInit(): void {
    this.repoSubs = this.repoService.getPages().subscribe(
      data => {
        this.repositories = data;
      }
      , error => {this.toastService.error(error.message, error.status);}
    );
  }

  ngOnDestroy(): void {
    this.repoSubs.unsubscribe();
  }

  deleteRow(id: string|undefined) : void {
    this.repoSubs = this.repoService.deleteById(id!).subscribe({
      next: data => {
        const index = this.repositories.findIndex(m => m.id == id);
        this.repositories.splice(index, 1);
      }
    });
  }  
}
