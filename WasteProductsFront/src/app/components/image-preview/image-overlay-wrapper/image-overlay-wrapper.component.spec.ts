import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImageOverlayWrapperComponent } from './image-overlay-wrapper.component';

describe('ImageOverlayWrapperComponent', () => {
  let component: ImageOverlayWrapperComponent;
  let fixture: ComponentFixture<ImageOverlayWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImageOverlayWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImageOverlayWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
