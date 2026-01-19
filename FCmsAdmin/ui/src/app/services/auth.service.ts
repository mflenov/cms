import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken?: string;
  expiresIn?: number;
  user?: any;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'cms_jwt_token';
  private readonly REFRESH_TOKEN_KEY = 'cms_refresh_token';
  
  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  public isLoggedIn$ = this.isLoggedInSubject.asObservable();

  constructor(private httpClient: HttpClient) {}

  /**
   * Login user and store JWT token
   */
  login(credentials: LoginCredentials): Observable<LoginResponse> {
    const loginUrl = environment.baseurl + 'v1/auth/login';
    
    return this.httpClient.post<LoginResponse>(environment.apiCmsServiceEndpoint + loginUrl, credentials).pipe(
      tap(response => {
        if (response.token) {
          this.setToken(response.token);
          if (response.refreshToken) {
            this.setRefreshToken(response.refreshToken);
          }
          this.isLoggedInSubject.next(true);
        }
      }),
      catchError(this.handleError)
    );
  }

  /**
   * Logout user and clear stored tokens
   */
  logout(): void {
    this.clearTokens();
    this.isLoggedInSubject.next(false);
  }

  /**
   * Get the current JWT token
   */
  getToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(this.TOKEN_KEY);
    }
    return null;
  }

  /**
   * Get the refresh token
   */
  getRefreshToken(): string | null {
    if (typeof window !== 'undefined') {
      return localStorage.getItem(this.REFRESH_TOKEN_KEY);
    }
    return null;
  }

  /**
   * Check if user has a valid token
   */
  hasToken(): boolean {
    const token = this.getToken();
    if (!token) return false;
    
    try {
      // Check if token is expired (basic check)
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp * 1000; // Convert to milliseconds
      return Date.now() < exp;
    } catch (error) {
      return false;
    }
  }

  /**
   * Check if user is currently logged in
   */
  isLoggedIn(): boolean {
    return this.hasToken();
  }

  /**
   * Refresh the JWT token
   */
  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return throwError({ status: 401, message: 'No refresh token available' });
    }

    const refreshUrl = environment.baseurl + 'v1/auth/refresh';
    
    return this.httpClient.post<LoginResponse>(environment.apiCmsServiceEndpoint + refreshUrl, { refreshToken }).pipe(
      tap(response => {
        if (response.token) {
          this.setToken(response.token);
          if (response.refreshToken) {
            this.setRefreshToken(response.refreshToken);
          }
        }
      }),
      catchError(this.handleError)
    );
  }

  /**
   * Set the JWT token in localStorage
   */
  private setToken(token: string): void {
    if (typeof window !== 'undefined') {
      localStorage.setItem(this.TOKEN_KEY, token);
    }
  }

  /**
   * Set the refresh token in localStorage
   */
  private setRefreshToken(refreshToken: string): void {
    if (typeof window !== 'undefined') {
      localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    }
  }

  /**
   * Clear all stored tokens
   */
  private clearTokens(): void {
    if (typeof window !== 'undefined') {
      localStorage.removeItem(this.TOKEN_KEY);
      localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    }
  }

  /**
   * Handle HTTP errors
   */
  private handleError(error: HttpErrorResponse) {
    let errorMessage = '';
    
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `${error.message}`;
    }
    return throwError({ status: error.status, message: errorMessage });
  }
}