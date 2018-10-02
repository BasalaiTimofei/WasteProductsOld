import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormProductOverlayComponent } from './form-product-overlay.component';

describe('FormProductOverlayComponent', () => {
  let component: FormProductOverlayComponent;
  let fixture: ComponentFixture<FormProductOverlayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ FormProductOverlayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormProductOverlayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
