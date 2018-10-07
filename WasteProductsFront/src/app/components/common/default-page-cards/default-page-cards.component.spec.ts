import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DefaultPageCardsComponent } from './default-page-cards.component';

describe('DefaultPageCardsComponent', () => {
  let component: DefaultPageCardsComponent;
  let fixture: ComponentFixture<DefaultPageCardsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DefaultPageCardsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DefaultPageCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
