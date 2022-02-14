import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

import { IFilterModel } from '../models/filter-model';
import { IApiRequestModel } from '../models/api-request-model'


@Injectable()

export class FiltersService {
  private listurl: string = 'api/v1/config/filters';
  private url: string = 'api/v1/config/filter/';

  constructor(private httpClient: HttpClient) {

  }

  getFilters(): Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listurl).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  getById(id: string): Observable<IFilterModel> {
    return this.httpClient.get<IFilterModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.url + id).pipe(
      catchError(this.handleError)
    );
  }

  save(model: IFilterModel): Observable<any> {
    if (model.id)
      return this.httpClient.put(environment.apiCmsServiceEndpoint + this.url, model);
    else
      return this.httpClient.post(environment.apiCmsServiceEndpoint + this.url, model);
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