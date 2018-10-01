import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/products/product';
import { Router } from '@angular/router';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  addProduct(){
    this.router.navigate(['products/add-product']);
  }
}
