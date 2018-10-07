import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material';
import { GroupModel, GroupInfoModel } from '../../models/group';
import { GroupDialogInfoComponent } from '../group-dialog-info/group-dialog-info.component';
import { GroupsService } from '../../services/groups.service';
import { BoardInfoModel } from '../../models/board';
import { BoardService } from '../../services/board.service';
import { BoardDialogInfoComponent } from '../board-dialog-info/board-dialog-info.component';
import { AuthenticationService } from '../../../account/services/authentication.service';


@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {

  group: GroupModel = new GroupModel();

  constructor(
    private aothService: AuthenticationService,
    private route: ActivatedRoute,
    private boardService: BoardService,
    private groupsService: GroupsService,
    private dialog: MatDialog) { }

  ngOnInit() {

    this.update();
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

    dialogRef.afterClosed().subscribe(result => {
      this.updateGroupInfo(result);
    });
  }

  updateGroupInfo(groupInfo: GroupInfoModel) {
    this.groupsService.updateGroup(this.group.Id, groupInfo).subscribe(group => this.group = Object.assign(this.group, groupInfo));
  }

  update() {
    const groupId = this.route.snapshot.paramMap.get('id');
    this.groupsService.getGroup(groupId).subscribe(group => this.group = group);
  }

  addBoard() {
    const dialogRef = this.dialog.open<BoardDialogInfoComponent, { action: string, data: BoardInfoModel }, BoardInfoModel>(
      BoardDialogInfoComponent, {
        // width: '250px',
        data: {
          action: 'Создать',
          data: new BoardInfoModel()
        }
      });

    dialogRef.afterClosed().subscribe(boardInfo => {
      if (boardInfo) {
        boardInfo.CreatorId = '0';// this.aothService.getUserId();

        this.boardService.createBoard(this.group.Id, boardInfo).subscribe(boardModel => {
          if (boardModel) {
            this.group.GroupBoards.push(boardModel);
            this.update();
          }
        }, () => { console.log(); });
      }
    }, () => { });

  }

}
