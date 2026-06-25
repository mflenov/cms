import { Component, OnDestroy, OnInit, signal } from '@angular/core';
import { Subscription } from 'rxjs';

import { DbconnectionsService } from '../../../services/dbconnections.service';
import { IDbConnectionModel } from 'src/app/models/dbconnection-model';
import { ToastService } from 'src/app/shared/services/toast.service';

@Component({
  selector: 'app-dbconnections',
  templateUrl: './dbconnections.component.html',
  styleUrl: './dbconnections.component.css',
  providers: [DbconnectionsService, ToastService],
  standalone: false
})
export class DbconnectionsComponent implements OnInit, OnDestroy {
  private _dbConnections = signal<IDbConnectionModel[]>([]);
  dbConnections = this._dbConnections.asReadonly();
  connectionSubs!: Subscription;

  constructor(
    private dbconnectionsService: DbconnectionsService,
    private toastService: ToastService
  ) {

  }

  ngOnInit(): void {
    this.connectionSubs = this.dbconnectionsService.getDbConnections().subscribe(
        dbConnections => {
            this._dbConnections.set(dbConnections.data as IDbConnectionModel[]);
        }
        , error => {this.toastService.error(error.message, error.status);}
      );
  }

  ngOnDestroy() : void {
    this.connectionSubs.unsubscribe();
  }

  deleteRow(id: string|undefined): void {
    this.dbconnectionsService.deleteById(id!).subscribe({
      next: result => {
        this._dbConnections.update(dbConnections => dbConnections.filter(m => m.id !== id));
      }
    });
  }
}
