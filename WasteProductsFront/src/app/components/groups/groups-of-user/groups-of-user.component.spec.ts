import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupsOfUserComponent } from './groups-of-user.component';

describe('GroupsOfUserComponent', () => {
  let component: GroupsOfUserComponent;
  let fixture: ComponentFixture<GroupsOfUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupsOfUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupsOfUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
