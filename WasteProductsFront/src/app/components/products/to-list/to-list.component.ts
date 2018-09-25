import { Component, OnInit } from '@angular/core';
import { UserProduct } from '../../../models/users/user-product';
import { ProductService } from '../../../services/product/product.service';

@Component({
  selector: 'app-to-list',
  templateUrl: './to-list.component.html',
  styleUrls: ['./to-list.component.css']
})
export class ToListComponent implements OnInit {

  userProducts: UserProduct[];

  constructor(private srv: ProductService) { }

  ngOnInit() {
    this.srv.loadUserProducts().subscribe(
    res => this.userProducts = res,
    err => console.error(err));
  }

}
