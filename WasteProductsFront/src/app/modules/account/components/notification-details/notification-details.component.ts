import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
/* Services */
import { NotificationService } from '../../services/notification.service';
/* Models */
import { Notification } from '../../models/notification';

@Component({
  selector: 'app-notification-details',
  templateUrl: './notification-details.component.html',
  styleUrls: ['./notification-details.component.css']
})
export class NotificationDetailsComponent implements OnInit {

  notfication: Notification;

  constructor(private route: ActivatedRoute,
    private notificationService: NotificationService,
    private location: Location) { }

  ngOnInit() {
    const notificationId = this.route.snapshot.paramMap.get('id');
    this.getNotification(notificationId);
  }

  getNotification(notificationId: string): void {
    this.notificationService.getById(notificationId).subscribe(notfication => {
      this.notfication = notfication;
      if (!notfication.Read) {
        this.notificationService.markAsRead(notfication.Id);
      }
    });
  }

  goBack(): void {
    this.location.back();
  }
}
