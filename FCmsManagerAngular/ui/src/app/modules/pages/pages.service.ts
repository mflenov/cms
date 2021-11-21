import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { IPageModel } from './models/pagemodel';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

@Injectable()

export class PagesService {
  private listUrl: string = 'api/v1/pages';

  constructor(private httpClient: HttpClient) 
  { }

  getPages(): Observable<IPageModel[]> {
    return this.httpClient.get<IPageModel[]>(environment.apiCmsServiceEndpoint + this.listUrl).pipe(
      tap(),
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
