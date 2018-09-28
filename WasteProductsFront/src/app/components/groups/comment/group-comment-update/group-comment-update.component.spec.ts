import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupCommentUpdateComponent } from './group-comment-update.component';

describe('GroupCommentUpdateComponent', () => {
  let component: GroupCommentUpdateComponent;
  let fixture: ComponentFixture<GroupCommentUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupCommentUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupCommentUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
