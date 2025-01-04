import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.css']
})
export class ToastComponent implements OnInit, OnDestroy {
  toasts: any[] = [];
  private subscription!: Subscription;

  constructor(private toastService: ToastService) {}

  ngOnInit() {
    this.subscription = this.toastService.getToasts().subscribe({
      next: (toast) => {
        this.toasts.push(toast);
        
        if (toast.delay) {
          setTimeout(() => this.removeToast(toast), toast.delay);
      }
    }});
  }

  removeToast(toast: any) {
    const index = this.toasts.indexOf(toast);
    if (index > -1) {
      this.toasts.splice(index, 1);
    }
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}