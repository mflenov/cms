import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { IContentModel } from '../models/content.model';
import { IContentStructureModel } from '../models/content-structure.model';
import { environment } from 'src/environments/environment';
import { catchError, tap } from 'rxjs/operators';

import { IApiRequestModel } from 'src/app/models/api-request-model'
import { INewContentModel } from '../models/new-content.model';

@Injectable()

export class ContentService {
  private listUrl: string = environment.baseurl + 'v1/repositories/Content';
  private url: string = environment.baseurl + 'v1/repository/Content';
  private definitionurl: string = environment.baseurl + 'v1/repository/structure/';

  constructor(private httpClient: HttpClient) 
  { }

  getContents(): Observable<IContentModel[]> {
    return this.httpClient.get<IContentModel[]>(environment.apiCmsServiceEndpoint + this.listUrl).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  getContent(id: string): Observable<IApiRequestModel> {
    return this.httpClient.get<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.definitionurl + id).pipe(
      tap(),
      catchError(this.handleError)
    );
  }

  deleteById(id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.url + id);
  }

  save(model: IContentStructureModel): Observable<any> {
    return this.httpClient.patch(environment.apiCmsServiceEndpoint + this.url, model);
  }

  create(model: INewContentModel): Observable<any> {
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
