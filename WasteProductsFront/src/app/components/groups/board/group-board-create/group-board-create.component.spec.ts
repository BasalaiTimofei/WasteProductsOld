import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupBoardCreateComponent } from './group-board-create.component';

describe('GroupBoardCreateComponent', () => {
  let component: GroupBoardCreateComponent;
  let fixture: ComponentFixture<GroupBoardCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupBoardCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupBoardCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
