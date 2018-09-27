import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GroupUserGetEntitleComponent } from './group-user-get-entitle.component';

describe('GroupUserGetEntitleComponent', () => {
  let component: GroupUserGetEntitleComponent;
  let fixture: ComponentFixture<GroupUserGetEntitleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GroupUserGetEntitleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GroupUserGetEntitleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
