import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { IApiRequestModel } from '../models/api-request-model';
import { environment } from '../../environments/environment';
import { catchError, tap } from 'rxjs/operators';

@Injectable()

export class DbconnectionsService {
  private listurl: string = 'api/v1/config/dbconnections';
  private url: string = 'api/v1/config/dbconnection/';
  private cache: IApiRequestModel | undefined;

  constructor(private httpClient: HttpClient) { }

  getDbConnections() : Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listurl)
      .pipe(
        tap(m => { this.cache = m; }),
        catchError(this.handleError)        
      );
  }

  handleError(err: HttpErrorResponse): Observable<never> {
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      // network error
      errorMessage = `An error occurred: ${err.error.message}`;
    } else {
      // bad response code
      errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
    }
    console.error(errorMessage);
    return throwError(errorMessage);
  }
}
