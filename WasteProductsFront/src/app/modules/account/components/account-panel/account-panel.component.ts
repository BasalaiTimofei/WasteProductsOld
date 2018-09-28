import { Component, OnInit } from '@angular/core';
import { MatBottomSheetRef } from '@angular/material';
import { OAuthService, OAuthErrorEvent } from 'angular-oauth2-oidc';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-account-panel',
  templateUrl: './account-panel.component.html',
  styleUrls: ['./account-panel.component.css']
})
export class AccountPanelComponent implements OnInit {

  public constructor(private authService: AuthenticationService,
    private bottomSheetRef: MatBottomSheetRef<AccountPanelComponent>) { }

  ngOnInit() {
  }

  protected logOut(event: MouseEvent) {
    this.authService.logOut();
  }

  protected openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }
}
