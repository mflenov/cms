import { Component, OnDestroy, OnInit, signal } from '@angular/core';
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
  private _users = signal<IUserModel[]>([]);
  users = this._users.asReadonly();
  connectionSubs!: Subscription;

  constructor(
    private userrsService: UsersService,
    private toastService: ToastService
  ) {

  }

  ngOnInit(): void {
    this.connectionSubs = this.userrsService.getUsers().subscribe(
        dbConnections => {
            this._users.set(dbConnections.data as IUserModel[]);
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
        this._users.update(users => users.filter(m => m.id !== id));
      }
    });
  }
}
