import { TestBed } from '@angular/core/testing';

import { CmsenumsService } from './cmsenums.service';

describe('CmsenumsService', () => {
  let service: CmsenumsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CmsenumsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
