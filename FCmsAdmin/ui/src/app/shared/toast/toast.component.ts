import { Component, OnInit, OnDestroy } from '@angular/core';
import { ToastService } from '../services/toast.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-toast',
  template: `
    <div class="toast-container position-fixed top-0 end-0 p-3">
      <div 
        *ngFor="let toast of toasts"
        class="toast show" 
        [ngClass]="toast.classname"
        role="alert" 
        aria-live="assertive" 
        aria-atomic="true">
        <div class="toast-header">
          <strong class="me-auto">Уведомление</strong>
          <button 
            type="button" 
            class="btn-close" 
            (click)="removeToast(toast)">
          </button>
        </div>
        <div class="toast-body">
          {{toast.message}}
        </div>
      </div>
    </div>
  `,
  styles: [`
    .toast-container {
      z-index: 1200;
    }
  `]
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