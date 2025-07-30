import { Component, OnDestroy, OnInit } from '@angular/core';
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
  dbConnections: IDbConnectionModel[] = [];
  connectionSubs!: Subscription;

  constructor(
    private dbconnectionsService: DbconnectionsService,
    private toastService: ToastService
  ) {

  }

  ngOnInit(): void {
    this.connectionSubs = this.dbconnectionsService.getDbConnections().subscribe(
        dbConnections => {
            this.dbConnections = dbConnections.data as IDbConnectionModel[];
        }
        , error => {this.toastService.error(error.message, error.status);}
      );
  }

  ngOnDestroy() : void {
    this.connectionSubs.unsubscribe();
  }

  deleteRow(id: string|undefined): void {
    console.log('deleteRow', id);
    this.dbconnectionsService.deleteById(id!).subscribe({
      next: result => {
        const index = this.dbConnections.findIndex(m => m.id == id);
        this.dbConnections.splice(index, 1);
      }
    });
  }
}
