import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupCommentCreateComponent } from './group-comment-create.component';

describe('GroupCommentCreateComponent', () => {
  let component: GroupCommentCreateComponent;
  let fixture: ComponentFixture<GroupCommentCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupCommentCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupCommentCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
