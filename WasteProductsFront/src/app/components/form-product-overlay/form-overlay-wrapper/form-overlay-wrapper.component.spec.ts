import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormOverlayWrapperComponent } from './form-overlay-wrapper.component';

describe('FormOverlayWrapperComponent', () => {
  let component: FormOverlayWrapperComponent;
  let fixture: ComponentFixture<FormOverlayWrapperComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormOverlayWrapperComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormOverlayWrapperComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
