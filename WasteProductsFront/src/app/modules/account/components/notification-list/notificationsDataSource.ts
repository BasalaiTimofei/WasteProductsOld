import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';
import { BehaviorSubject, Observable } from 'rxjs';
/* Services */
import { NotificationService } from '../../services/notification.service';
/* Models */
import { Notification } from '../../models/notification';

export class NotificationsDataSource implements DataSource<Notification> {

  private notificationsSubject = new BehaviorSubject<Notification[]>([]);
  private loadingSubject = new BehaviorSubject<boolean>(false);

  loading$ = this.loadingSubject.asObservable();
  notificationsCount$ = this.notificationService.notificationsCount$;

  constructor(private notificationService: NotificationService) { }

  connect(collectionViewer: CollectionViewer): Observable<Notification[]> {
    return this.notificationsSubject.asObservable();
  }

  disconnect(collectionViewer: CollectionViewer): void {
    this.notificationsSubject.complete();
    this.loadingSubject.complete();
  }

  loadNotifications(pageIndex = 0, pageSize = 10) {
    this.loadingSubject.next(true);

    this.notificationService.getByPage(pageIndex, pageSize).subscribe(
      notifications => {
        this.notificationsSubject.next(notifications);
        this.loadingSubject.next(false);
      },
      error => {
        this.notificationsSubject.next([]);
        this.loadingSubject.next(false);
      });
  }
}
