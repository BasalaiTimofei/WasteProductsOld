import { Component, OnInit } from '@angular/core';
import { ProductDescription } from '../../../models/users/product-description';
import { ProductService } from '../../../services/product/product.service';

@Component({
  selector: 'app-to-list',
  templateUrl: './to-list.component.html',
  styleUrls: ['./to-list.component.css']
})
export class ToListComponent implements OnInit {

  products: ProductDescription[];

  constructor(private srv: ProductService) { }

  ngOnInit() {
    this.srv.loadProducts().subscribe(
    res => this.products = res,
    err => console.error(err));
  }

}
