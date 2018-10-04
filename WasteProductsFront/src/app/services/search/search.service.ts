import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';
import { ProductService } from '../product/product.service';

// models
import { SearchProduct } from '../../models/search-product';
import { UserQuery } from '../../models/top-query';

// environment
import { environment } from '../../../environments/environment';
import { UserProduct } from '../../models/users/user-product';


@Injectable({
  providedIn: 'root'
})
export class SearchService extends BaseHttpService {
  private URL_SEARCH = `${environment.apiHostUrl}/api/search`;  // URL to web api

  public userProductsId: UserProduct[];
  public searchProducts: SearchProduct[];

  public constructor(httpService: HttpClient,
                    loggingService: LoggingService,
                    public productService: ProductService) {
    super(httpService, loggingService);
    this.productService.getUserProducts().toPromise().then(
      res => {
        this.userProductsId = res;
      } ,
      err => console.error(err));
  }

  getDefault(query: string): Observable<SearchProduct[]> {
    return this.httpService.get<SearchProduct[]>(this.URL_SEARCH + '/products/default', this.getOptions(query)).pipe(
      map(res => {
        if (res !== null) {
        const result: any = res;
        return result.map((item) => new SearchProduct(item.Id, item.Name, this.checkExistInUserProducts(item.Id), item.PicturePath));
        }
      }), catchError(this.handleError('Error in search.service getDefault()', []))
    );
  }

  getTopSearchQueries(query: string): Observable<UserQuery[]> {
    return this.httpService.get<UserQuery[]>(this.URL_SEARCH + '/queries', this.getOptions(query)).pipe(
      map(res => {
        const result: any = res;
        return result.map(item => new UserQuery(item.QueryString));
      }),
      catchError(this.handleError('getTopSearchQueries', []))
    );
  }

  private getOptions(query: string) {
    return {
      params: new HttpParams().set('query', query)
    };
  }

  private checkExistInUserProducts(id: string): boolean {
      return this.userProductsId.some(function(item) {
        return item.Product.Id === id;
      });
  }
}
