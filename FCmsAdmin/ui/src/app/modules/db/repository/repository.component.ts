import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IDbRepositoryModel } from '../models/dbrepository.model';
import { DbRepoService } from '../services/dbrepo.service';

@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['./repository.component.css'],
  providers: [DbRepoService]
})

export class RepositoryComponent implements OnInit, OnDestroy {
  repositories: IDbRepositoryModel[] = [];
  repoSubs!: Subscription;

  constructor(private repoService: DbRepoService) { }

  ngOnInit(): void {
    this.repoSubs = this.repoService.getPages().subscribe({
      next: data => {
        this.repositories = data;
      }
    });
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
