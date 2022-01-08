import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { IPageModel } from './models/page.model';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

import { IApiRequest } from '../../models/api-request-model'

@Injectable()

export class PagesService {
  private listUrl: string = 'api/v1/pages';
  private url:  string = 'api/v1/page/';
  private definitionurl: string = 'api/v1/page/structure/';

  constructor(private httpClient: HttpClient) 
  { }

  getPages(): Observable<IPageModel[]> {
    return this.httpClient.get<IPageModel[]>(environment.apiCmsServiceEndpoint + this.listUrl).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  getPage(id: string): Observable<IApiRequest> {
    return this.httpClient.get<IApiRequest>(environment.apiCmsServiceEndpoint + this.definitionurl + id).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequest> {
    return this.httpClient.delete<IApiRequest>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  handleError(err: HttpErrorResponse):Observable<never>  {
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
