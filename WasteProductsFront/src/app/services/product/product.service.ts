import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';

// environment
import { environment } from '../../../environments/environment.prod';
import { UserProduct } from '../../models/users/user-product';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthenticationService } from '../../modules/account/services/authentication.service';
import { ProductDescription } from '../../models/products/product-description';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends BaseHttpService {
  constructor(httpService: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpService, loggingService);
    this.baseProdApiUrl = `${environment.apiHostUrl}/api/products`;
    this.baseUserApiUrl = `${environment.apiHostUrl}/api/user`;
  }

  private baseProdApiUrl;
  private baseUserApiUrl;

  createProduct(rating: number, description: string) {
    const createProdUrl = this.baseProdApiUrl;
    let productId: string;
    this.httpService.post(createProdUrl, null).subscribe(res => productId = <string>res, err => console.error(err));

    const addProdUrl = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/products/${productId}`;

    const descr = new ProductDescription();
    descr.Rating = rating;
    descr.Description = description;

    this.httpService.post(addProdUrl, descr);
  }

  addProductDescription(rating: number, descrText: string, productId: string) {
    const description = new ProductDescription();
    description.Rating = rating;
    description.Description = descrText;

    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/products/${productId}`;
    this.httpService.post(url, description)
    .subscribe(
      res => console.log(res),
      err => console.log(err));
  }

  getUserProducts() {
    // const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/products`; // MyStubs
    const url = `${this.baseUserApiUrl}/0/products`;
    return this.httpService.get<UserProduct[]>(url);
   }

   updateUserProduct(productId: string, rating: number, descrText: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/products/${productId}`;

    const description = new ProductDescription();
    description.Rating = rating;
    description.Description = descrText;

    this.httpService.put(url, description);
   }

   deleteUserProduct(productId: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/products/${productId}`;
    this.httpService.delete(url)
    .subscribe(
      res => console.log(res),
      err => console.log(err));
   }

   getAllProducts(): Observable<UserProduct[]> {
    const url = this.baseProdApiUrl;

    return this.httpService.get<UserProduct[]>(url)
    .pipe(
      tap(data => this.logDebug('fetched products')),
      catchError(this.handleError('getProducts', []))
      );
  }
}
