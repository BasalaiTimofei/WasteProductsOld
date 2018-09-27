import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';

// environment
import { environment } from '../../../environments/environment.prod';
import { UserProduct } from '../../models/users/user-product';
import { Product } from '../../models/products/product';
import { Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { AuthenticationService } from '../../modules/account/services/authentication.service';
import { ProductDescription } from '../../models/products/product-description';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends BaseHttpService {

  PRODUCTS_DATA: Product[] = [
    {Id: '1', Name: 'Колбаса', AvgRating: 3, Composition: 'drthdrthydrtyh', IsHidden: false},
    {Id: '2', Name: 'Гречка', AvgRating: 5, Composition: 'djyhdtyjhtyj', IsHidden: false},
    {Id: '3', Name: 'Хлеб острошицкий', AvgRating: 1, Composition: 'rthdrht', IsHidden: false},
    {Id: '4', Name: 'Сгущеное молоко', AvgRating: 3, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '5', Name: 'Ваниль', AvgRating: 4.3, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '6', Name: 'Напиток', AvgRating: 4, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '7', Name: 'Вода "Святой источник"', AvgRating: 5, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '8', Name: 'Егурт Dagnon', AvgRating: 3.3, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '9', Name: 'Сметана', AvgRating: 1.2, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '10', Name: 'Брокколи консервинованные', AvgRating: 4.9, Composition: 'drhjdrhdryth', IsHidden: true},
    {Id: '11', Name: 'Чай черный', AvgRating: 4.8, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '12', Name: 'Чай зеленый', AvgRating: 5, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '13', Name: 'Саморезы', AvgRating: 5, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '14', Name: 'Соль йодированная', AvgRating: 2.1, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '15', Name: 'Шуруповерт', AvgRating: 3, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '16', Name: 'Шуруповерт', AvgRating: 5, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '17', Name: 'Кефир', AvgRating: 3.5, Composition: 'drhjdrhdryth', IsHidden: false},
    {Id: '18', Name: 'Сахар', AvgRating: 5, Composition: 'rthdrht', IsHidden: false},
    {Id: '19', Name: 'Фисташки', AvgRating: 4.5, Composition: 'rthdrht', IsHidden: false},
    {Id: '20', Name: 'Calcium', AvgRating: 5, Composition: 'rthdrht', IsHidden: false},
  ];

  private apiUrl = `${environment.apiHostUrl}/api/product/products`;

  constructor(httpService: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpService, loggingService);
  }

  getUserProducts() {
    const claims = this.authServise.getClaims();
    const url = `${environment.apiHostUrl}/api/user/0/products`;
    return this.httpService.get<UserProduct[]>(url);
   }

   updateProduct(productId: string, rating: number, description: string) {
    const claims = this.authServise.getClaims();
    const url = `${environment.apiHostUrl}/api/user/${claims.sub}/updateproductdescription/${productId}`;
    // tslint:disable-next-line:prefer-const
    let descr: ProductDescription;
    descr.Rating = rating;
    descr.Description = description;
    descr.Rating = rating;

    this.httpService.put(url, descr);
   }

   getProducts(): Observable<UserProduct[]> {
    const url = `${this.apiUrl}/products`; // правишь урл под конкретный запрос От Сани Галговского

    return this.httpService.get<UserProduct[]>(url)
    .pipe(
      tap(data => this.logDebug('fetched products')),
      catchError(this.handleError('getProducts', []))
      );
  }
}
