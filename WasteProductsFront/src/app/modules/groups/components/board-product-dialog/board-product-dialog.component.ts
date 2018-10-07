import { Component, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { ProductInfoModel } from '../../models/product';

@Component({
  selector: 'app-board-product-dialog',
  templateUrl: './board-product-dialog.component.html',
  styleUrls: ['./board-product-dialog.component.css']
})
export class BoardProductDialogComponent implements OnInit {

  model: ProductInfoModel = new ProductInfoModel();

  constructor(
    private dialogRef: MatDialogRef<BoardProductDialogComponent, ProductInfoModel>) {

  }

  ngOnInit() {
  }

  productSelected(productId: string) {
    this.model.ProductId = productId;
  }

}
