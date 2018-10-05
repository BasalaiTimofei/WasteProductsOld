import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllToListComponent } from './all-to-list.component';

describe('AllToListComponent', () => {
  let component: AllToListComponent;
  let fixture: ComponentFixture<AllToListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllToListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllToListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
