import { TestBed } from '@angular/core/testing';

import { FormPreviewService } from './form-preview.service';

describe('FormPreviewService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: FormPreviewService = TestBed.get(FormPreviewService);
    expect(service).toBeTruthy();
  });
});
