import { Component, OnInit } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GroupsService } from '../../services/groups.service';
import { AuthenticationService } from '../../../account/services/authentication.service';

import { GroupModel, GroupOfUserModel, GroupInfoModel } from '../../models/group';
import { MatDialog } from '@angular/material';
import { GroupDialogInfoComponent } from '../group-dialog-info/group-dialog-info.component';

@Component({
  selector: 'app-groups',
  templateUrl: './groups.component.html',
  styleUrls: ['./groups.component.css']
})
export class GroupsComponent implements OnInit {

  private userId: string;
  private groupsSubject: BehaviorSubject<GroupOfUserModel[]> = new BehaviorSubject<GroupOfUserModel[]>([]);

  myGroup: GroupModel;
  otherGroups: Observable<GroupOfUserModel[]> = this.groupsSubject.asObservable();

  constructor(authService: AuthenticationService, private groupsService: GroupsService, private dialog: MatDialog) {
    this.userId = authService.getUserId(); // this.authService.getUserId();
  }

  ngOnInit() {
    this.update();
  }

  createGroup() {
    const dialogRef = this.dialog.open<GroupDialogInfoComponent, { action: string, data: GroupInfoModel }, GroupInfoModel>(
      GroupDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Create',
          data: { Name: 'My super group', Information: ':)' }
        }
      });

    dialogRef.afterClosed().subscribe(result => {
      this.groupsService.crateGroup(this.userId, result).subscribe(myGroup => {
        this.myGroup = myGroup;
      });
    });
  }

  private update() {
    if (this.userId) {
      this.groupsService.getUserGroup(this.userId).subscribe(group => this.myGroup = group);
      this.groupsService.getUserOtherGroups(this.userId).subscribe(groups => this.groupsSubject.next(groups));
    }
  }
}
