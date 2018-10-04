import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MaterialModule } from 'src/app/modules/material/material.module';
/* Components */
import { BoardComponent } from './components/board/board.component';
import { GroupComponent } from './components/group/group.component';
import { BoardListComponent } from './components/board-list/board-list.component';
import { GroupListComponent } from './components/group-list/group-list.component';
import { GroupUserListComponent } from './components/group-user-list/group-user-list.component';
/* Dialogs */
import { BoardDialogAddComponent } from './components/board-dialog-add/board-dialog-add.component';
import { BoardDialogRemoveComponent } from './components/board-dialog-remove/board-dialog-remove.component';
import { GroupDialogUserInviteComponent } from './components/group-dialog-user-invite/group-dialog-user-invite.component';
import { GroupDialogUserKickComponent } from './components/group-dialog-user-kick/group-dialog-user-kick.component';
import { GroupDialogInfoComponent } from './components/group-dialog-info/group-dialog-info.component';
import { GroupsComponent } from './components/groups/groups.component';



@NgModule({
  imports: [
    CommonModule, RouterModule, FormsModule,
    MaterialModule,

  ],
  declarations: [
    GroupsComponent,
    GroupListComponent,
    GroupComponent, GroupUserListComponent, GroupDialogUserInviteComponent, GroupDialogUserKickComponent, GroupDialogInfoComponent,
    BoardComponent, BoardListComponent, BoardDialogAddComponent, BoardDialogRemoveComponent, GroupsComponent,
  ],
  entryComponents: [
    GroupDialogUserInviteComponent, GroupDialogUserKickComponent, GroupDialogInfoComponent,
    BoardDialogAddComponent, BoardDialogRemoveComponent,
  ],
  exports: [GroupsComponent, GroupComponent, GroupListComponent, GroupsComponent]
})
export class GroupsModule { }
