import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDialogUserInviteComponent } from './group-dialog-user-invite.component';

describe('GroupDialogUserInviteComponent', () => {
  let component: GroupDialogUserInviteComponent;
  let fixture: ComponentFixture<GroupDialogUserInviteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupDialogUserInviteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupDialogUserInviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
