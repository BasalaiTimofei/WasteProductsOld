import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticationService } from '../../services/authentication.service';
import { NotificationService } from '../../services/notification.service';
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

  isAuthenificated$: Observable<boolean>;
  hasUnreadNotifications$: Observable<boolean>;

  constructor(
    private bottomSheet: MatBottomSheet,
    private authService: AuthenticationService,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.isAuthenificated$ = this.authService.isAuthenticated$;
    this.hasUnreadNotifications$ = this.notificationService.hasUnreadNotifications$;
  }

  logIn(event: Event) {
    this.authService.logIn();
  }

  openSheet(): void {
    this.bottomSheet.open(AccountPanelComponent);
  }
}
