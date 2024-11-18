import { TestBed } from '@angular/core/testing';

import { ImageBlobService } from './image-blob.service';

describe('ImageBlobService', () => {
  let service: ImageBlobService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ImageBlobService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
