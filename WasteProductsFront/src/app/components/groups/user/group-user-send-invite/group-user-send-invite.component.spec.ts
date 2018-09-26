import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserSendInviteComponent } from './group-user-send-invite.component';

describe('GroupUserSendInviteComponent', () => {
  let component: GroupUserSendInviteComponent;
  let fixture: ComponentFixture<GroupUserSendInviteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserSendInviteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserSendInviteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
