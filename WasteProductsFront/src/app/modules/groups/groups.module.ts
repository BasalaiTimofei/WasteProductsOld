import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from 'src/app/modules/material/material.module';
import { GroupsRoutingModule } from './groups.router';
/* Components */
import { BoardComponent } from './components/board/board.component';
import { GroupComponent } from './components/group/group.component';
/* Dialogs */
import { GroupDialogInfoComponent } from './components/group-dialog-info/group-dialog-info.component';
import { GroupsComponent } from './components/groups/groups.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { LightSearchComponent } from '../../components/light-search/light-search.component';
import { BoardProductDialogComponent } from './components/board-product-dialog/board-product-dialog.component';
import { BoardDialogInfoComponent } from './components/board-dialog-info/board-dialog-info.component';
import { BoardProductDialogUpdateComponent } from './components/board-product-dialog-update/board-product-dialog-update.component';




@NgModule({
    imports: [
        CommonModule, FormsModule,
        MaterialModule,

        GroupsRoutingModule,
    ],
    declarations: [
        GroupsComponent,
        GroupComponent, GroupDialogInfoComponent, BoardProductDialogComponent,
        BoardComponent, GroupsComponent, ConfirmDialogComponent,
        LightSearchComponent,
        BoardDialogInfoComponent,
        BoardProductDialogUpdateComponent,

    ],
    entryComponents: [
        GroupDialogInfoComponent,
        ConfirmDialogComponent, BoardProductDialogComponent, BoardDialogInfoComponent, BoardProductDialogUpdateComponent,
    ]
})
export class GroupsModule { }
