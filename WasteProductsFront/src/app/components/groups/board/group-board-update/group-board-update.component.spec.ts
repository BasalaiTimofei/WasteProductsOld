import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupBoardUpdateComponent } from './group-board-update.component';

describe('GroupBoardUpdateComponent', () => {
  let component: GroupBoardUpdateComponent;
  let fixture: ComponentFixture<GroupBoardUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupBoardUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupBoardUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
