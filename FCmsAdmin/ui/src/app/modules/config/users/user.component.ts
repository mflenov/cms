import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { IUserModel } from 'src/app/models/user-model';
import { UsersService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  providers: [UsersService],
  standalone: false,
})
export class UserComponent {
  model: IUserModel = {
    username: '',
    password: '',
  };

  modelSubs!: Subscription;

  constructor(private usersService: UsersService,
    private route: ActivatedRoute,
    private router: Router) {
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.modelSubs = this.usersService.getById(id).subscribe({
        next: model => { this.model = model }
      });
    }
  }

  ngOnDestroy(): void {
    if (this.modelSubs) {
      this.modelSubs.unsubscribe();
    }
  }

  onSubmit(form: NgForm): void {
    this.usersService.save(this.model).subscribe({
      next: data => {
        this.router.navigate(['/config/users']);
      }
    });
  }
}
