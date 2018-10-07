import { Component, ViewChild, OnInit, EventEmitter, Input, Output } from '@angular/core';
import { Product } from '../../../models/products/product';
import { MatTableDataSource, MatPaginator, MatSort } from '@angular/material';
import { TableDataSource, ValidatorService } from 'angular4-material-table';
import { ProductService } from '../../../services/product/product.service';
import { UserProduct } from '../../../models/users/user-product';
import { Router } from '@angular/router';
import { ProductsComponent } from '../products.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { FormPreviewOverlay } from '../../form-product-overlay/form-preview-overlay';
import { FormPreviewService } from '../../../services/form-preview/form-preview.service';


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
    private previewDialogForm: FormPreviewService,
    private router: Router) {}

  products: Product[] = [];
  userProducts: UserProduct[] = [];
  defualtImage = '../assets/img/tenor.gif';

  @Input() input = this.userProducts ;
  @Output() personListChange = new EventEmitter<Product[]>();

  data: UserProduct[] = this.userProducts;
  dataSource = new MatTableDataSource<UserProduct>();
  displayedColumns: string[] = ['Name', 'Rating', 'Description', 'IsHidden'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;


  ngOnInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

this.productService.getUserProducts().subscribe(
    res => {
      for (const item of res) {
        if (item.Product.PicturePath === 'http://waste-api.belpyro.net/Content/favicon.png') {
           item.Product.PicturePath = this.defualtImage;
         }
      }
      this.userProducts = res;
      this.dataSource.data = res;
    },
    err => console.error(err));
  }

  applyFilter(filterValue: string) {
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

    addProduct() {
      this.router.navigate(['products/add-product']);
    }

    editSlectedProduct(productId: string) {
      const dialog: FormPreviewOverlay = this.previewDialogForm.open({
        form: { name: 'Редактировать Мои продукты', id: productId, editMode: true }
      });
    }
  }
