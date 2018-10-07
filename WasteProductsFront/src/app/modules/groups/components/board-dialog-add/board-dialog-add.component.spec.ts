import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardDialogAddComponent } from './board-dialog-add.component';

describe('BoardDialogAddComponent', () => {
  let component: BoardDialogAddComponent;
  let fixture: ComponentFixture<BoardDialogAddComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardDialogAddComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardDialogAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
