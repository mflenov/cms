import { Component, OnInit, OnDestroy } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { Observable, of, Subscription } from 'rxjs';
import { INewDbRepoModel } from '../models/new-dbrepo.model';
import { DbRepoService } from '../services/dbrepo.service';
import { IDbConnectionModel } from 'src/app/models/dbconnection-model';
import { DbconnectionsService } from 'src/app/services/dbconnections.service';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
    selector: 'app-new-repo',
    templateUrl: './new-repo.component.html',
    styleUrls: ['./new-repo.component.css'],
    providers: [DbRepoService, DbconnectionsService],
    standalone: false
})
export class NewRepoComponent implements OnInit, OnDestroy {
  private connectionSubs!: Subscription;

  model: INewDbRepoModel = {} as INewDbRepoModel;
  dbConnections!: Observable<IDbConnectionModel[]>;
  

  constructor(
    private dbconnectionsService: DbconnectionsService,
    private repoService: DbRepoService,
    private router: Router,
    private toastService: ToastService
  ) { }

  ngOnInit(): void {
    this.connectionSubs = this.dbconnectionsService.getDbConnections().subscribe(
      dbConnections => { this.dbConnections = of(dbConnections.data); }
      , error => {this.toastService.error(error.message, error.status);});
  }

  ngOnDestroy() : void {
    this.connectionSubs.unsubscribe();
  }

  onSubmit(form: NgForm): void {
    this.repoService.create(this.model).subscribe({
      next: data => {
        this.router.navigate(['/db']);
      }
    });
  }
}
