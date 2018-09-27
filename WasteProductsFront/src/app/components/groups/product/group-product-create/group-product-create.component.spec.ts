import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupProductCreateComponent } from './group-product-create.component';

describe('GroupProductCreateComponent', () => {
  let component: GroupProductCreateComponent;
  let fixture: ComponentFixture<GroupProductCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupProductCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
