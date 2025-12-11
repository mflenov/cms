import { TestBed } from '@angular/core/testing';

import { RepositoryItemService } from './repository-item.service';

describe('RepositoryItemService', () => {
  let service: RepositoryItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RepositoryItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
