import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AccountPanelButtonComponent } from './account-panel-button.component';

describe('AccountPanelButtonComponent', () => {
  let component: AccountPanelButtonComponent;
  let fixture: ComponentFixture<AccountPanelButtonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AccountPanelButtonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AccountPanelButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
