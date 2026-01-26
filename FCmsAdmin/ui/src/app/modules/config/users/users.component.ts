import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

import { ToastService } from 'src/app/shared/services/toast.service';
import { IUserModel } from '../../../models/user-model';
import { UsersService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
  providers: [UsersService, ToastService],
  standalone: false
})
export class UsersComponent implements OnInit, OnDestroy {
  users: IUserModel[] = [];
  connectionSubs!: Subscription;

  constructor(
    private userrsService: UsersService,
    private toastService: ToastService
  ) {

  }

  ngOnInit(): void {
    this.connectionSubs = this.userrsService.getUsers().subscribe(
        dbConnections => {
            this.users = dbConnections.data as IUserModel[];
        }
        , error => {this.toastService.error(error.message, error.status);}
      );
  }

  ngOnDestroy() : void {
    this.connectionSubs.unsubscribe();
  }

  deleteRow(id: string|undefined): void {
    this.userrsService.deleteById(id!).subscribe({
      next: result => {
        const index = this.users.findIndex(m => m.id == id);
        this.users.splice(index, 1);
      }
    });
  }
}
