import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserDismissUserComponent } from './group-user-dismiss-user.component';

describe('GroupUserDismissUserComponent', () => {
  let component: GroupUserDismissUserComponent;
  let fixture: ComponentFixture<GroupUserDismissUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserDismissUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserDismissUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
