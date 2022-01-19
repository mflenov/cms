import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHandler } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';

import { environment } from 'src/environments/environment';
import { IApiRequestModel } from 'src/app/models/api-request-model'
import { IPageContentModel } from '../models/page-content.model';


@Injectable({
    providedIn: 'root'
})

export class ContentService {
    private editpageurl: string = 'api/v1/content';

    constructor(private httpClient: HttpClient) {
    }

    getPageContent(id: string): Observable<IApiRequestModel> {
        const headers = { 
            'content-type': 'application/json',
            'accept': 'text/plain' 
        } ;
        return this.httpClient.post<IApiRequestModel>(environment.apiCmsServiceEndpoint + this.editpageurl, 
            {
                repositoryid: id,
                filters: null
            },
            { 'headers' : headers });
    }

    save(model: IPageContentModel): Observable<any> {
        return this.httpClient.put(environment.apiCmsServiceEndpoint + this.editpageurl, model);
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