import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LightSearchComponent } from './light-search.component';

describe('LightSearchComponent', () => {
  let component: LightSearchComponent;
  let fixture: ComponentFixture<LightSearchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LightSearchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LightSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
