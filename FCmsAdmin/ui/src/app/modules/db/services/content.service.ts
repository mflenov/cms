import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable()

export class ContentService {
  private listContent: string = 'api/v1/db/content';

  constructor(private httpClient: HttpClient) { }
}
