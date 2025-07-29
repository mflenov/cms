import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { IApiRequestModel } from '../models/api-request-model';
import { environment } from '../../environments/environment';
import { catchError, tap } from 'rxjs/operators';
import { IDbConnectionModel } from '../models/dbconnection-model';

@Injectable()

export class DbconnectionsService {
  private listurl: string = environment.baseurl + 'v1/config/dbconnections';
  private url: string = environment.baseurl + 'v1/config/dbconnection/';
  private cache: IApiRequestModel | undefined;

  constructor(private httpClient: HttpClient) { }

  getDbConnections() : Observable<IApiRequestModel> {
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
    return this.getDbConnections();
  }

  getById(id: string): Observable<IDbConnectionModel> {
    return this.httpClient.get<IDbConnectionModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  save(model: IDbConnectionModel): Observable<any> {
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
