import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/modules/material/material.module';
import { GroupsRoutingModule } from './groups.router';
/* Components */
import { BoardComponent } from './components/board/board.component';
import { GroupComponent } from './components/group/group.component';
import { BoardListComponent } from './components/board-list/board-list.component';
/* Dialogs */
import { BoardDialogAddComponent } from './components/board-dialog-add/board-dialog-add.component';
import { BoardDialogRemoveComponent } from './components/board-dialog-remove/board-dialog-remove.component';
import { GroupDialogInfoComponent } from './components/group-dialog-info/group-dialog-info.component';
import { GroupsComponent } from './components/groups/groups.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';




@NgModule({
  imports: [
    CommonModule, FormsModule,
    MaterialModule,

    GroupsRoutingModule,
  ],
  declarations: [
    GroupsComponent,
    GroupComponent, GroupDialogInfoComponent,
    BoardComponent, BoardListComponent, BoardDialogAddComponent, BoardDialogRemoveComponent, GroupsComponent, ConfirmDialogComponent,
  ],
  entryComponents: [
    GroupDialogInfoComponent,
    BoardDialogAddComponent, BoardDialogRemoveComponent, ConfirmDialogComponent,
  ]
})
export class GroupsModule { }
