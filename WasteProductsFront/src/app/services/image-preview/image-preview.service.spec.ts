import { TestBed } from '@angular/core/testing';

import { ImagePreviewService } from './image-preview.service';

describe('ImagePreviewService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ImagePreviewService = TestBed.get(ImagePreviewService);
    expect(service).toBeTruthy();
  });
});
