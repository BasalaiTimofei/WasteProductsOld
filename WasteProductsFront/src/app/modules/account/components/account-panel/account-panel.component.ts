import { Component, OnInit } from '@angular/core';
import { MatBottomSheetRef } from '@angular/material';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../../services/authentication.service';
import { NotificationService } from '../../services/notification.service';
import { map } from 'rxjs/operators';


@Component({
  selector: 'app-account-panel',
  templateUrl: './account-panel.component.html',
  styleUrls: ['./account-panel.component.css']
})
export class AccountPanelComponent implements OnInit {

  hasUnreadNotifications$: Observable<boolean>;
  unreadNotifcationsCount$: Observable<number>;

  constructor(private authService: AuthenticationService,
    private bottomSheetRef: MatBottomSheetRef<AccountPanelComponent>,
    private notificationService: NotificationService) { }

  ngOnInit() {
    this.hasUnreadNotifications$ = this.notificationService.hasUnreadNotifications$;
    this.unreadNotifcationsCount$ = this.notificationService.unreadNotifications$.pipe(map(notifications => notifications.length));
  }

  protected logOut(event: MouseEvent) {
    this.authService.logOut();
  }

  protected openLink(event: MouseEvent): void {
    this.bottomSheetRef.dismiss();
    event.preventDefault();
  }
}
