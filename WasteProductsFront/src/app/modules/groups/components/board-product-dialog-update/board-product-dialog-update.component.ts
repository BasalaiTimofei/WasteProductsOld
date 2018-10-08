import { Component, OnInit, Inject } from '@angular/core';
import { ProductInfoModel } from '../../models/product';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

@Component({
  selector: 'app-board-product-dialog-update',
  templateUrl: './board-product-dialog-update.component.html',
  styleUrls: ['./board-product-dialog-update.component.css']
})
export class BoardProductDialogUpdateComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<BoardProductDialogUpdateComponent>,
    @Inject(MAT_DIALOG_DATA) public model: ProductInfoModel) { }

  ngOnInit() {
  }

}
