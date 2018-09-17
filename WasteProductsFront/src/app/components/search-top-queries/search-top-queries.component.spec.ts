import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchTopQueriesComponent } from './search-top-queries.component';

describe('SearchTopQueriesComponent', () => {
  let component: SearchTopQueriesComponent;
  let fixture: ComponentFixture<SearchTopQueriesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchTopQueriesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchTopQueriesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
