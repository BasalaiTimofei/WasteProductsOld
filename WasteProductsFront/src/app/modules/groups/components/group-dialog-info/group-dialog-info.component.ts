import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { GroupInfoModel } from '../../models/group';

@Component({
  selector: 'app-group-dialog-info',
  templateUrl: './group-dialog-info.component.html',
  styleUrls: ['./group-dialog-info.component.css']
})
export class GroupDialogInfoComponent implements OnInit {

  constructor(
    private dialogRef: MatDialogRef<GroupDialogInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public model: { action: string, data: GroupInfoModel}) { }

  ngOnInit() {

  }

}
