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

  userProducts: UserProduct[]; // К обсуждению

  data: Product[] = this.productService.PRODUCTS_DATA;
  dataSource = new MatTableDataSource(this.productService.PRODUCTS_DATA);
  displayedColumns: string[] = ['id', 'name', 'avgRating', 'composition', 'isHidden'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;


  ngOnInit() {
    this.paginator.length = this.data.length;
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

this.productService.loadUserProducts().subscribe(
    res => this.userProducts = res, // К обсуждению
    err => console.error(err));
  }
    applyFilter(filterValue: string) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }
  }
