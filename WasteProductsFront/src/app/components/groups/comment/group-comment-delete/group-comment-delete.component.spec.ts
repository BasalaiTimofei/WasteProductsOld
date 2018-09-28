import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupCommentDeleteComponent } from './group-comment-delete.component';

describe('GroupCommentDeleteComponent', () => {
  let component: GroupCommentDeleteComponent;
  let fixture: ComponentFixture<GroupCommentDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupCommentDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupCommentDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
