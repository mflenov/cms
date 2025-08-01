import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

import { IFilterModel } from '../models/filter-model';
import { IApiRequestModel } from '../models/api-request-model'


@Injectable()

export class FiltersService {
  private listurl: string = environment.baseurl + 'v1/config/filters';
  private url: string = environment.baseurl + 'v1/config/filter/';

  private cache: IApiRequestModel | undefined;

  constructor(private httpClient: HttpClient) {

  }

  getFilters(): Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listurl).pipe(
      tap(m => { this.cache = m; }),
      catchError(this.handleError)
    );
  }

  getCachedFilters(): Observable<IApiRequestModel> {
    if (this.cache) {
      return of(this.cache);
    }
    return this.getFilters();
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