import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardProductDialogComponent } from './board-product-dialog.component';

describe('BoardProductDialogComponent', () => {
  let component: BoardProductDialogComponent;
  let fixture: ComponentFixture<BoardProductDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardProductDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardProductDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
