import { Component, ViewChild, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../models/products/product';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { TableDataSource, ValidatorService } from 'angular4-material-table';
import { ProductService } from '../../../services/product/product.service';
import { UserProduct } from '../../../models/users/user-product';
import { Router } from '@angular/router';
import { ProductsComponent } from '../products.component';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-to-list',
  templateUrl: './to-list.component.html',
  styleUrls: ['./to-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],

  providers: [ProductService]
})

export class ToListComponent implements OnInit {

  constructor (public productService: ProductService,
    private router: Router) {}

  products: Product[] = [];
  userProducts: UserProduct[] = [];
  @Input() input = this.userProducts ;
  @Output() personListChange = new EventEmitter<Product[]>();

  data: UserProduct[] = this.userProducts;
  dataSource = new MatTableDataSource<UserProduct>();
  displayedColumns: string[] = ['Name', 'AvgRating', 'Composition', 'IsHidden'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  datasourceSubject: TableDataSource<Product>;

  ngOnInit() {
    this.paginator.length = this.data.length;
    this.dataSource.sort = this.sort;

this.productService.getUserProducts().subscribe(
    res => {
      this.userProducts = res;
      this.dataSource.data = res;
    },
    err => console.error(err));
  }

    applyFilter(filterValue: any) {
      this.dataSource.filter = filterValue.trim().toLowerCase();
    }

    addProduct() {
      this.router.navigate(['products/add-product']);
    }
  }
