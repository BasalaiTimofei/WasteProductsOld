import { Injectable } from '@angular/core';
import { SignalR, ISignalRConnection, IConnectionOptions } from 'ng2-signalr';
import { Observable, BehaviorSubject } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { isEqual } from 'lodash';
/* Services */
import { BaseService } from 'src/app/services/base/base.service';
import { LoggingService } from 'src/app/services/logging/logging.service';
import { AuthenticationService } from './authentication.service';
/* Models */
import { Notification } from '../models/notification';
/* Environment */
import { environment } from 'src/environments/environment';



@Injectable({
  providedIn: 'root'
})
export class NotificationService extends BaseService {

  private _hub: ISignalRConnection;
  private _notificationsSubject: BehaviorSubject<Notification[]>;

  notifications$: Observable<Notification[]>;
  unreadNotifications$: Observable<Notification[]>;

  notificationsCount$: Observable<number>;
  unreadNotificationsCount$: Observable<number>;

  hasUnreadNotifications$: Observable<boolean>;

  constructor(private authService: AuthenticationService, signalR: SignalR, loggingService: LoggingService) {
    super(loggingService);

    this._notificationsSubject = new BehaviorSubject<Notification[]>([]);

    this.notifications$ = this._notificationsSubject.asObservable();
    this.unreadNotifications$ = this.notifications$.pipe(map(notifications => notifications.filter(n => !n.Read)));

    this.notificationsCount$ = this.notifications$.pipe(map(notifications => notifications.length));
    this.unreadNotificationsCount$ = this.unreadNotifications$.pipe(map(notifications => notifications.length));

    this.hasUnreadNotifications$ = this.unreadNotificationsCount$.pipe(map(count => count > 0));

    this.configure(signalR);
  }

  getByPage(pageIndex: number, pageSize: number = 10): Observable<Notification[]> {
    return this.notifications$.pipe(map(notifications => {
      const startIndex = pageIndex * pageSize;
      return notifications.slice(startIndex, startIndex + pageSize);
    }));
  }

  getById(id: string): Observable<Notification> {
    return this.notifications$.pipe(
      map(notifications => {
        const notification = notifications.find(n => n.Id === id);

        if (!notification) {
          throw new Error(`Notification with id: ${id} doesn't exists`);
        }

        return notification;
      })
    );
  }

  markAsRead(id: string) {
    return this.getById(id)
      .pipe(tap(n => this._hub.invoke('MarkReadAsync', this.authService.getUserId(), n.Id)))
      .subscribe(n => n.Read = true);
  }

  private configure(signalR: SignalR) {
    // create options
    const options: IConnectionOptions = {
      url: environment.apiHostUrl,
      hubName: 'NotificationHub',
    };
    // create connection to hub
    this._hub = signalR.createConnection(options);

    this._hub.listenFor<Notification[]>('ReciveNotifications')
      .subscribe(notifications => {
        if (!isEqual(this._notificationsSubject.value, notifications)) {
          this._notificationsSubject.next(notifications);
        }
      }, this.logError);

    this._hub.listenFor<Notification>('ReciveNotification')
      .subscribe(notification => {
        const notifications = this._notificationsSubject.value;
        notifications.push(notification);
        this._notificationsSubject.next(notifications);
      }, this.logError);

    this.authService.isAuthenticated$.subscribe(isAuthenticated => {
      if (isAuthenticated) {
        this.connect();
      } else {
        this.disconnect();
      }
    });
  }

  private connect() {
    // Set auth headers.
    const $ = (<any>window).$;
    $.signalR.ajaxDefaults.headers = {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + this.authService.getAccessToken()
    };

    this._hub.start()
      .then(() => this.logDebug('Connection to notification hub established.'))
      .catch(this.logError);
  }

  private disconnect() {
    this._hub.stop();
  }
}
