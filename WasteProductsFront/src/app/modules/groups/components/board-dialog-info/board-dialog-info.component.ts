import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { BoardInfoModel } from '../../models/board';

@Component({
  selector: 'app-board-dialog-info',
  templateUrl: './board-dialog-info.component.html',
  styleUrls: ['./board-dialog-info.component.css']
})
export class BoardDialogInfoComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<BoardDialogInfoComponent>,
    @Inject(MAT_DIALOG_DATA) public model: { action: string, data: BoardInfoModel}) { }

  ngOnInit() {
  }

}
