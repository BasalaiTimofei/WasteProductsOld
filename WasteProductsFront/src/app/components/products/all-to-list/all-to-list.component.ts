import { Component, ViewChild, OnInit } from '@angular/core';
import { Product } from '../../../models/products/product';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { ProductService } from '../../../services/product/product.service';
import { Router } from '@angular/router';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-all-to-list',
  templateUrl: './all-to-list.component.html',
  styleUrls: ['./all-to-list.component.css'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
  
  providers: [ProductService]
})
export class AllToListComponent implements OnInit {

  products: Product[] = [];
  defualtImage: string = '../assets/img/tenor.gif';
  dataSource = new MatTableDataSource<Product>();
  displayedColumns: string[] = ['Id', 'Name', 'AvgRating'];
  headerNames: string[] = ['Id', 'Наименование', 'Средний рейтинг'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor (public productService: ProductService,
    private router: Router) {}

  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

    this.productService.getProducts().subscribe(
      res => {
        for (let item of res) {
          if (item.PicturePath == "http://waste-api.belpyro.net/Content/favicon.png") item.PicturePath = this.defualtImage; 
        }
        this.products = res;
        this.dataSource.data = res;
      },
      err => console.error(err)
    );
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
}
