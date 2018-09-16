import { TestBed } from '@angular/core/testing';

import { MainPageService } from './main-page.service';

describe('MainPageService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MainPageService = TestBed.get(MainPageService);
    expect(service).toBeTruthy();
  });
});
