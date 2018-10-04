import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupDialogUserKickComponent } from './group-dialog-user-kick.component';

describe('GroupDialogUserKickComponent', () => {
  let component: GroupDialogUserKickComponent;
  let fixture: ComponentFixture<GroupDialogUserKickComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupDialogUserKickComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupDialogUserKickComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
