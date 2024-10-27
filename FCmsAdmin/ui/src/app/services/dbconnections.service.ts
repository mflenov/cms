import { Injectable } from '@angular/core';

@Injectable()

export class DbconnectionsService {
  private listurl: string = 'api/v1/config/dbconnections';
  private url: string = 'api/v1/config/dbconnection/';

  constructor() { }
}
