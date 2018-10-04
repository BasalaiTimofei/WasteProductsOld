import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserListComponent } from './group-user-list.component';

describe('GroupUserListComponent', () => {
  let component: GroupUserListComponent;
  let fixture: ComponentFixture<GroupUserListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
