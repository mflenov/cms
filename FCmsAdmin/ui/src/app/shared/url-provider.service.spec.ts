import { TestBed } from '@angular/core/testing';

import { UrlProviderService } from './url-provider.service';

describe('UrlProviderService', () => {
  let service: UrlProviderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UrlProviderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
