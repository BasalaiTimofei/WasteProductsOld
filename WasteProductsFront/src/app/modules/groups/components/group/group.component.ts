import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { GroupModel, GroupInfoModel } from '../../models/group';
import { GroupDialogInfoComponent } from '../group-dialog-info/group-dialog-info.component';
import { GroupsService } from '../../services/groups.service';


@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {

  group: GroupModel = new GroupModel();

  constructor(
    private route: ActivatedRoute,
    private groupsService: GroupsService,
    private dialog: MatDialog) { }

  ngOnInit() {
    const groupId = this.route.snapshot.paramMap.get('id');
    this.groupsService.getGroup(groupId).subscribe(group => this.group = group);
  }

  edit() {
    const dialogRef = this.dialog.open<GroupDialogInfoComponent, { action: string, data: GroupInfoModel }, GroupInfoModel>(
      GroupDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Update',
          data: Object.assign(new GroupInfoModel(), this.group)
        }
      });

    dialogRef.afterClosed().subscribe(result => this.updateInfo(result));
  }

  updateInfo(groupInfo: GroupInfoModel) {
    this.groupsService.updateInfo(this.group.Id, groupInfo).subscribe(group => this.group = group);
  }

}
