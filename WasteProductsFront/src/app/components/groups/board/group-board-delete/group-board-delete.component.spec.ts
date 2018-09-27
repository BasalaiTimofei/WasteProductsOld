import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupBoardDeleteComponent } from './group-board-delete.component';

describe('GroupBoardDeleteComponent', () => {
  let component: GroupBoardDeleteComponent;
  let fixture: ComponentFixture<GroupBoardDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupBoardDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupBoardDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
