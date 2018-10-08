import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BoardProductDialogUpdateComponent } from './board-product-dialog-update.component';

describe('BoardProductDialogUpdateComponent', () => {
  let component: BoardProductDialogUpdateComponent;
  let fixture: ComponentFixture<BoardProductDialogUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BoardProductDialogUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BoardProductDialogUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
