import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupProductUpdateComponent } from './group-product-update.component';

describe('GroupProductUpdateComponent', () => {
  let component: GroupProductUpdateComponent;
  let fixture: ComponentFixture<GroupProductUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupProductUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupProductUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
