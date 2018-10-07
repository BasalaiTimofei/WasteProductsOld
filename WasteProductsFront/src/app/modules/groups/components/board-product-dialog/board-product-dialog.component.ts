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
  selectedProduct: any;

  constructor(
    private dialogRef: MatDialogRef<BoardProductDialogComponent, ProductInfoModel>) {

  }

  ngOnInit() {
  }

  productSelected(product: any) {
    this.model.ProductId = product.Id;
    this.model.Name = product.Name;
    this.selectedProduct = product;
  }

}
