import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { Observable, throwError, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators'

import { IEnumsModel } from './enums-model'
import { environment } from '../../../environments/environment';


@Injectable({
  providedIn: 'root'
})


export class CmsenumsService {
  private url = 'api/v1/data/enums'
  private enumModel?: IEnumsModel

  constructor(private httpClient: HttpClient) { 
  }

  getEnums(): Observable<IEnumsModel> {
    if (this.enumModel) {
      return of(this.enumModel!);
    }

    return this.httpClient.get<IEnumsModel>(environment.apiCmsServiceEndpoint + this.url).pipe(
      tap(data => this.enumModel = data),
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
