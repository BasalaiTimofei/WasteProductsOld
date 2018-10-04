import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardDialogRemoveComponent } from './board-dialog-remove.component';

describe('BoardDialogRemoveComponent', () => {
  let component: BoardDialogRemoveComponent;
  let fixture: ComponentFixture<BoardDialogRemoveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardDialogRemoveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardDialogRemoveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
