import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { IApiRequestModel } from '../models/api-request-model';
import { environment } from '../../environments/environment';
import { catchError, tap } from 'rxjs/operators';
import { IUserModel } from '../models/user-model';

@Injectable()

export class UsersService {
  private listurl: string = environment.baseurl + 'v1/config/users/';
  private url: string = environment.baseurl + 'v1/config/user/';
  private cache: IApiRequestModel | undefined;

  constructor(private httpClient: HttpClient) { }

  getUsers() : Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listurl)
      .pipe(
        tap(m => { this.cache = m; }),
        catchError(this.handleError)        
      );
  }

  getCachedConnections(): Observable<IApiRequestModel> {
    if (this.cache) {
      return of(this.cache);
    }
    return this.getUsers();
  }

  getById(id: string): Observable<IUserModel> {
    return this.httpClient.get<IUserModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  save(model: IUserModel): Observable<any> {
    if (model.id)
      return this.httpClient.patch(environment.apiCmsServiceEndpoint + this.url, model);
    else
      return this.httpClient.put(environment.apiCmsServiceEndpoint + this.url, model);
  }

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
