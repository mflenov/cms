import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { IApiRequestModel } from '../../../models/api-request-model';
import { IDbRowModel } from '../models/dncontent.model';


@Injectable()

export class DbContentService {
  private listContent: string = environment.baseurl + 'v1/dbcontent';
  private editContent: string = environment.baseurl + 'v1/dbcontent';

  constructor(
      private httpClient: HttpClient
    ) { }

  getDbContent(id: string): Observable<IApiRequestModel> {
    const headers = {
      'content-type': 'application/json',
      'accept': 'text/plain'
    };
    return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listContent,
      {
        repositoryid: id
      },
      { 'headers': headers })
      .pipe(
        catchError(this.handleError)
      );
  }

  save(repositoryId: string, model: IDbRowModel): Observable<any> {
    return this.httpClient.put(environment.apiCmsServiceEndpoint + this.editContent, {
      repositoryId: repositoryId,
      row: model
    })
    .pipe(
      catchError(this.handleError)
    );
  }

  delete(repositoryId: string, id: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.editContent +
      "?repositoryid=" + repositoryId + "&id=" + id)
    .pipe(
      catchError(this.handleError)
    );
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
