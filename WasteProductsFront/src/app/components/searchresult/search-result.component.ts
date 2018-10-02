import { Component, OnDestroy } from '@angular/core';
import { PageEvent } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject, BehaviorSubject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material';

import { SearchProduct } from '../../models/search-product';
import { SearchService } from '../../services/search/search.service';
import { ProductService } from '../../services/product/product.service';
import { FormPreviewService } from '../../services/form-preview/form-preview.service';
import { FormPreviewOverlay } from '../form-product-overlay/form-preview-overlay';
import { ImagePreviewService } from '../../services/image-preview/image-preview.service';
import { ImagePreviewOverlay } from '../image-preview/image-preview-overlay';
import { AuthenticationService } from '../../modules/account/services/authentication.service';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnDestroy {
  private destroy$ = new Subject<void>();
  public isAuth: boolean;

  responseMessage = 'Продукт удален из Мои продукты';
  query: string;
  searchResult: SearchProduct[];
  statusCode: number;
  tempProducts: SearchProduct[];
  errorMessage: string;
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  constructor(private searchService: SearchService,
              private productService: ProductService,
              private route: ActivatedRoute,
              private previewDialogForm: FormPreviewService,
              private previewDialog: ImagePreviewService,
              public snackBar: MatSnackBar,
              public authService: AuthenticationService) {

      this.route.params.pipe(takeUntil(this.destroy$)).subscribe(({ query }: Params) => {
        if (!query) {
            return;
      }
      this.authService.isAuthenticated$.toPromise<boolean>().then(res => {
        this.isAuth = res;
      });
      this.setVariablesToDefault();
      this.search(query);
    });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public search(query: string): void {
    this.searchService.getDefault(query).toPromise().then((data) => {
        this.searchResult = data;

        if (!this.searchResult) {
            return;
        }

        this.length = this.searchResult.length;
        this.changePageEvent();
    }).catch((e: HttpErrorResponse) => {
        this.errorMessage = 'Поиск не дал результатов...';
        if (e.status === 204) {
            this.statusCode = e.status;
        }
    });
  }

  addToMyProducts(productId: string) {
    const dialog: FormPreviewOverlay = this.previewDialogForm.open({
      form: { name: 'Добавить в Мой список', id: productId, searchQuery: this.query }
    });
  }

  removeFromMyProducts(productId: string) {
    this.productService.deleteUserProduct(productId);
    this.snackBar.open(this.responseMessage, null, {
      duration: 4000,
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
  }

  showPreview() {
    const dialog: ImagePreviewOverlay = this.previewDialog.open({
      // TODO Заменить путем из реквеста и название продукта
      image: { name: 'Название продукта', url: 'https://static.pexels.com/photos/371633/pexels-photo-371633.jpeg' }
    });
  }

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempProducts = this.searchResult.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1));
  return event;
  }

  private setVariablesToDefault() {
    this.pageSize = 5;
    this.pageIndex = 0;
    this.length = 0;
  }
}
