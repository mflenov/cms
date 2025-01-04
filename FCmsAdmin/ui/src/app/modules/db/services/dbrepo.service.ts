import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { IDbRepositoryModel } from '../models/dbrepository.model';
//import { IPageStructureModel } from '../models/page-structure.model';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

import { IApiRequestModel } from 'src/app/models/api-request-model'
import { INewDbRepoModel } from '../models/new-dbrepo.model';

@Injectable()

export class DbRepoService {
  private listUrl: string = 'api/v1/db';
  private url:  string = 'api/v1/db/';
  private definitionurl: string = 'api/v1/db/structure/';

  constructor(
    private httpClient: HttpClient
  ) 
  { }

  getPages(): Observable<IDbRepositoryModel[]> {
    return this.httpClient.get<IDbRepositoryModel[]>(environment.apiCmsServiceEndpoint + this.listUrl).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  getPage(id: string): Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.definitionurl + id).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.url + id);
  }

  create(model: INewDbRepoModel): Observable<any> {
    return this.httpClient.put(environment.apiCmsServiceEndpoint + this.url, model);
  }

  handleError = (err: HttpErrorResponse): Observable<never> => {
    debugger;
    let errorMessage = '';
    if (err.error instanceof ErrorEvent) {
      errorMessage = `Error: ${err.error.message}`;
    } else {
      errorMessage = `Server Code: ${err.status}, Message: ${err.message}`;
    }    
    return throwError(errorMessage);
  }
}
