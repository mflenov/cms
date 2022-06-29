import { TestBed } from '@angular/core/testing';

import { DbContentService } from './dbcontent.service';

describe('ContentService', () => {
  let service: DbContentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DbContentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
