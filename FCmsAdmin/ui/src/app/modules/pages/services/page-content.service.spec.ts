import { TestBed } from '@angular/core/testing';

import { PageContentService } from './page-content.service';

describe('ContentService', () => {
  let service: PageContentService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PageContentService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
