import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardDialogInfoComponent } from './board-dialog-info.component';

describe('BoardDialogInfoComponent', () => {
  let component: BoardDialogInfoComponent;
  let fixture: ComponentFixture<BoardDialogInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardDialogInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardDialogInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
