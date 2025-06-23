import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { DbconnectionsService } from '../../../services/dbconnections.service';
import { IDbConnectionModel } from 'src/app/models/dbconnection-model';

@Component({
    selector: 'app-dbconnections',
    templateUrl: './dbconnections.component.html',
    styleUrl: './dbconnections.component.css',
    providers: [DbconnectionsService],
    standalone: false
})
export class DbconnectionsComponent implements OnInit, OnDestroy {
  dbConnections: IDbConnectionModel[] = [];
  connectionSubs!: Subscription;

  constructor(private dbconnectionsService: DbconnectionsService) {

  }

  ngOnInit(): void {
    this.connectionSubs = this.dbconnectionsService.getDbConnections()
      .subscribe(
        {
          next: dbConnections => {
            this.dbConnections = dbConnections.data as IDbConnectionModel[];
          }
        }
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
