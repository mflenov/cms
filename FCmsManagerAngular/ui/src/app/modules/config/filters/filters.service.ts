import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

import { IFilterModel } from './filter-model';


@Injectable()

export class FiltersService {
  private listurl: string = 'api/v1/config/filters';
  private geturl:  string = 'api/v1/config/filter/';
  private saveUrl: string = 'api/v1/config/filter';

  constructor(private httpClient: HttpClient) {

  }

  getFilters(): Observable<IFilterModel[]> {
    return this.httpClient.get<IFilterModel[]>(environment.apiCmsServiceEndpoint + this.listurl).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  getById(id: string): Observable<IFilterModel> {
    return this.httpClient.get<IFilterModel>(environment.apiCmsServiceEndpoint + this.geturl + id).pipe(
      catchError(this.handleError)
    );
  }

  save(model: IFilterModel): Observable<any> {
    if (model.id)
      return this.httpClient.put(environment.apiCmsServiceEndpoint + this.saveUrl, model);
    else 
      return this.httpClient.post(environment.apiCmsServiceEndpoint + this.saveUrl, model);
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