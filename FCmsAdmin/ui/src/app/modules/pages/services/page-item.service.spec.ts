import { TestBed } from '@angular/core/testing';

import { PageItemService } from './page-item.service';

describe('ContentService', () => {
  let service: PageItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PageItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
