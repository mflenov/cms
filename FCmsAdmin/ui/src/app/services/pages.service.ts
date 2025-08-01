import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { IPageModel } from '../models/page.model';
import { IPageStructureModel } from '../models/page-structure.model';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

import { IApiRequestModel } from 'src/app/models/api-request-model'
import { INewPageModel } from '../models/new-page.model';

@Injectable()

export class PagesService {
  private listUrl: string = environment.baseurl + 'v1/repositories/Page';
  private url: string = environment.baseurl + 'v1/repository/Page/';
  private definitionurl: string = environment.baseurl + 'v1/repository/structure/';

  constructor(private httpClient: HttpClient) 
  { }

  getPages(): Observable<IPageModel[]> {
    return this.httpClient.get<IPageModel[]>(environment.apiCmsServiceEndpoint + this.listUrl).pipe(
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

  save(model: IPageStructureModel): Observable<any> {
    return this.httpClient.patch(environment.apiCmsServiceEndpoint + this.url, model);
  }

  create(model: INewPageModel): Observable<any> {
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
