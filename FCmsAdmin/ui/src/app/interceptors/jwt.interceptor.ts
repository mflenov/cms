import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, BehaviorSubject } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(null);

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Add JWT token to request if available
    const token = this.authService.getToken();
    
    if (token && this.authService.isLoggedIn()) {
      request = this.addTokenToRequest(request, token);
    }

    return next.handle(request).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse) {
          // Handle 401 Unauthorized errors
          if (error.status === 401 && token) {
            return this.handle401Error(request, next);
          }
          
          // Handle 403 Forbidden errors
          if (error.status === 403) {
            this.authService.logout();
            return throwError(error);
          }
        }
        
        return throwError(error);
      })
    );
  }

  /**
   * Add JWT token to the request headers
   */
  private addTokenToRequest(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
  }

  /**
   * Handle 401 Unauthorized errors by attempting to refresh the token
   */
  private handle401Error(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const refreshToken = this.authService.getRefreshToken();
      
      if (refreshToken) {
        return this.authService.refreshToken().pipe(
          switchMap((response: any) => {
            this.isRefreshing = false;
            this.refreshTokenSubject.next(response.token);
            
            // Retry the original request with the new token
            return next.handle(this.addTokenToRequest(request, response.token));
          }),
          catchError(error => {
            this.isRefreshing = false;
            
            // Refresh failed, logout user
            this.authService.logout();
            return throwError(error);
          })
        );
      } else {
        // No refresh token available, logout user
        this.authService.logout();
        return throwError({ status: 401, message: 'Authentication required' });
      }
    } else {
      // Token refresh is in progress, wait for it to complete
      return this.refreshTokenSubject.pipe(
        filter(token => token != null),
        take(1),
        switchMap(token => next.handle(this.addTokenToRequest(request, token)))
      );
    }
  }
}