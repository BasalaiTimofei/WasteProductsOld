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

  private apiUrl = `${environment.apiHostUrl}/api/product/products`;

  constructor(httpService: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpService, loggingService);
  }

  getUserProducts() {
    const url = `${environment.apiHostUrl}/api/user/0/products`;
    return this.httpService.get<UserProduct[]>(url);
   }

   updateUserProduct(productId: string, rating: number, description: string) {
    const url = `${environment.apiHostUrl}/api/user/${this.getUserId()}/products/${productId}`;

    const descr = new ProductDescription();
    descr.Rating = rating;
    descr.Description = description;

    this.httpService.put(url, descr);
   }

   deleteUserProduct(productId: string) {
    const url = `${environment.apiHostUrl}/api/user/${this.getUserId()}/products/${productId}`;
    this.httpService.delete(url);
   }

   getAllProducts(): Observable<UserProduct[]> {
    const url = `${this.apiUrl}/products`;

    return this.httpService.get<UserProduct[]>(url)
    .pipe(
      tap(data => this.logDebug('fetched products')),
      catchError(this.handleError('getProducts', []))
      );
  }

  private getUserId() {
    const claims = this.authServise.getClaims();
    return claims.sub;
  }
}
