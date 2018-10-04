import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserformfieldComponent } from './userformfield.component';

describe('UserformfieldComponent', () => {
  let component: UserformfieldComponent;
  let fixture: ComponentFixture<UserformfieldComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserformfieldComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserformfieldComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
