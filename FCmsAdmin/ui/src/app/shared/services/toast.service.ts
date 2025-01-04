import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface Toast {
  message: string;
  classname?: string;
  delay?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private static toasts$ = new Subject<Toast>();
  
  getToasts(): Observable<Toast> {
    return ToastService.toasts$.asObservable();
  }

  show(message: string, options: Partial<Toast> = {}) {
    const defaultOptions: Toast = {
      message,
      classname: 'bg-info text-light',
      delay: 3000
    };

    ToastService.toasts$.next({ ...defaultOptions, ...options });
  }

  success(message: string, options: Partial<Toast> = {}) {
    this.show(message, { classname: 'bg-success text-light', ...options });
  }

  error(message: string, options: Partial<Toast> = {}) {
    this.show(message, { classname: 'bg-danger text-light', ...options });
  }

  warning(message: string, options: Partial<Toast> = {}) {
    this.show(message, { classname: 'bg-warning text-dark', ...options });
  }
}