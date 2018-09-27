import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

import { AuthenticationService } from '../../services/authentication.service';

/* Materials */
import { MatBottomSheet } from '@angular/material';

/* Components */
import { AccountPanelComponent } from '../account-panel/account-panel.component';


@Component({
  selector: 'app-account-panel-button',
  templateUrl: './account-panel-button.component.html',
  styleUrls: ['./account-panel-button.component.css']
})
export class AccountPanelButtonComponent implements OnInit {

  protected isAuthenificated: Observable<boolean>;

  public constructor(
    private bottomSheet: MatBottomSheet,
    private authService: AuthenticationService) { }

  ngOnInit() {
    this.isAuthenificated = this.authService.isAuthenticated$;
  }

  protected logIn(event: Event) {
    this.authService.logIn();
  }

  protected openSheet(): void {
    this.bottomSheet.open(AccountPanelComponent);
  }
}
