import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import { catchError, tap } from 'rxjs/operators';

import { IApiRequest } from 'src/app/models/api-request-model'


@Injectable({
  providedIn: 'root'
})

export class ContentService {
  private editpageurl: string = 'api/v1/content/';

  constructor(private httpClient: HttpClient) { 
  }

  getPageContent(id: string): Observable<IApiRequest> {
    return this.httpClient.get<IApiRequest>(environment.apiCmsServiceEndpoint + this.editpageurl + id).pipe(
      tap(),
      catchError(this.handleError)
    );
  }
}