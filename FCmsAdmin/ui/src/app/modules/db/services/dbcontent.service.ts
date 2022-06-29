import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable} from 'rxjs';

import { environment } from 'src/environments/environment';
import { IApiRequestModel } from '../../../models/api-request-model';
import { IDbContentModel } from '../models/dncontent.model';

@Injectable()

export class DbContentService {
  private listContent: string = 'api/v1/db/content';

  constructor(private httpClient: HttpClient) { }

  getDbContent(id: string): Observable<IApiRequestModel> {
    const headers = {
      'content-type': 'application/json',
      'accept': 'text/plain'
    };
    return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.listContent,
      {
        repositoryid: id
      },
      { 'headers': headers });
  }
}
