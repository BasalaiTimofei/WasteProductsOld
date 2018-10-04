import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ConfirmEmailChangingComponent } from './confirm-email-changing.component';

describe('ConfirmEmailChangingComponent', () => {
  let component: ConfirmEmailChangingComponent;
  let fixture: ComponentFixture<ConfirmEmailChangingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ConfirmEmailChangingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ConfirmEmailChangingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
