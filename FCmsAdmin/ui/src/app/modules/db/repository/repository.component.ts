import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { IRepositoryModel } from '../models/repository.model';
import { RepoService } from '../services/repo.service';

@Component({
  selector: 'app-repository',
  templateUrl: './repository.component.html',
  styleUrls: ['./repository.component.css'],
  providers: [RepoService]
})

export class RepositoryComponent implements OnInit, OnDestroy {
  repositories: IRepositoryModel[] = [];
  repoSubs!: Subscription;

  constructor(private repoService: RepoService) { }

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