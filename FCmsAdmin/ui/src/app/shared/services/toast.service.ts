import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export interface Toast {
    title: string;
    message: string;
    classname?: string;
    delay?: number;
    errorCode?: number
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
        title: "Warning",
        message,
        classname: 'bg-info text-light',
        delay: 3000
    };

    ToastService.toasts$.next({ ...defaultOptions, ...options });
  }

  success(message: string, options: Partial<Toast> = {}) {
    this.show(message, { title: "Success", classname: 'bg-success text-light', ...options });
  }

  error(message: string, code: number, options: Partial<Toast> = {}) {
    this.show(message, { title: "Error", classname: 'bg-danger text-light', errorCode: code , ...options });
  }

  warning(message: string, options: Partial<Toast> = {}) {
    this.show(message, { classname: 'bg-warning text-dark', ...options });
  }
}