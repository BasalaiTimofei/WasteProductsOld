import { Component, ViewChild, OnInit } from '@angular/core';
import { Product } from '../../../models/products/product';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { ProductService } from '../../../services/product/product.service';
import { UserProduct } from '../../../models/users/user-product';

@Component({
  selector: 'app-to-list',
  templateUrl: './to-list.component.html',
  styleUrls: ['./to-list.component.css'],
  providers: [ProductService]
})

export class ToListComponent implements OnInit {

  constructor (private productService: ProductService) {}

  products: Product[] = [];
  userProducts: UserProduct[] = [];

  data: UserProduct[] = this.userProducts;
  dataSource = new MatTableDataSource(this.data);
  displayedColumns: string[] = ['Id', 'Name', 'AvgRating', 'Composition', 'IsHidden'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  ngOnInit() {
    this.paginator.length = this.data.length;
    this.dataSource.sort = this.sort;

this.productService.getUserProducts().subscribe(
    res => {
      this.userProducts = res;
      // tslint:disable-next-line:prefer-const
      for (let item of res) {
        item.Product.AvgRating = 3;
        item.Product.IsHidden = false;
        this.products.push(item.Product);
      }
    } ,
    err => console.error(err));
  }

    applyFilter(filterValue: any) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }
  }
