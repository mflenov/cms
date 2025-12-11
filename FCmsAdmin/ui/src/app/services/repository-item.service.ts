import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHandler } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';

import { environment } from 'src/environments/environment';
import { IApiRequestModel } from 'src/app/models/api-request-model'
import { IPageContentModel } from '../models/page-content.model';
import { IContentFilterModel } from '../models/content-filter.model';
import { IContentItemModel } from '../models/content-item.model';


@Injectable({
  providedIn: 'root'
})

export class RepositoryItemService {
  private editpageurl: string = environment.baseurl + 'v1/content';
  private filterpageurl: string = environment.baseurl + 'v1/content/filter';
  private listUrl: string = environment.baseurl + 'v1/content/list/';
  private contenturl: string = environment.baseurl + 'v1/contentitem';

  constructor(private httpClient: HttpClient) {
  }

  getContent(id: string, filters: IContentFilterModel[]): Observable<IApiRequestModel> {
    const headers = {
      'content-type': 'application/json',
      'accept': 'text/plain'
    };
    return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.editpageurl,
      {
        repositoryid: id,
        filters: filters
      },
      { 'headers': headers });
  }

  filterContent(id: string, filters: IContentFilterModel[] ): Observable<IApiRequestModel> {
    const headers = {
      'content-type': 'application/json',
      'accept': 'text/plain'
    };

    return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.filterpageurl,
      {
        repositoryid: id,
        filters: filters
      },
      { 'headers': headers });
  }

  listContent(repositoryId: string, definitionId: string, filters: IContentFilterModel[] = []): Observable<IApiRequestModel> {
    const headers = {
      'content-type': 'application/json',
      'accept': 'text/plain'
    };
    return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listUrl + repositoryId + "/" + definitionId,
      {
        filters: filters
      },
      { 'headers': headers });
  }

  save(model: IPageContentModel): Observable<any> {
    return this.httpClient.put(environment.apiCmsServiceEndpoint + this.editpageurl, model);
  }

  deleteById(repositoryId: string, contentid: string): Observable<IApiRequestModel> {
    return this.httpClient.delete<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.editpageurl + "/" + repositoryId + "/" + contentid);
  }

  updateItemById(repositoryid: string, model: IContentItemModel): Observable<any> {
    return this.httpClient.put(environment.apiCmsServiceEndpoint + this.contenturl + '/' + repositoryid, model);
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
