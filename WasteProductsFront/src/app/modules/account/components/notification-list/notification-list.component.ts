import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { MatPaginator } from '@angular/material';
import { Observable } from 'rxjs';
import { NotificationsDataSource } from './notificationsDataSource';

import { Notification } from '../../models/notification';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent implements OnInit, AfterViewInit {

  pageSize = 10;
  dataSource: NotificationsDataSource;
  displayedColumns = ['date', 'subject', 'message'];


  notifications$: Observable<Notification[]>;
  notificationsCount$: Observable<number>;

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private notificationService: NotificationService) {
  }

  ngOnInit() {
    this.dataSource = new NotificationsDataSource(this.notificationService);
    this.dataSource.loadNotifications();
  }

  ngAfterViewInit(): void {
    this.paginator.page.subscribe(pe => this.dataSource.loadNotifications(pe.pageIndex));
  }

  loadNotificationsPage() {
    this.dataSource.loadNotifications(this.paginator.pageIndex, this.pageSize);
  }
}
