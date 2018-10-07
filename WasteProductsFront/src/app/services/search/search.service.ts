import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

import { Observable, Subject } from 'rxjs';
import { catchError, map, filter } from 'rxjs/operators';

import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';
import { ProductService } from '../product/product.service';
import { AuthenticationService } from '../../modules/account/services/authentication.service';

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
  private URL_SEARCH = `${environment.apiHostUrl}/api/search`; // URL to web api

  public userProductsId: UserProduct[] = [];
  public searchProducts: SearchProduct[];

  public constructor(
    httpService: HttpClient,
    loggingService: LoggingService,
    public productService: ProductService,
    public authService: AuthenticationService ) {

      super(httpService, loggingService);
  }

  getDefaultAuth(query: string) {
    const products: Subject<SearchProduct[]> = new Subject<SearchProduct[]>();

    this.productService.getUserProducts().subscribe(userProducts => {
      const searchUrl = this.URL_SEARCH + '/products/default';

      this.httpService.get<SearchProduct[]>(searchUrl,  this.getOptions(query)).pipe(
          map(searchResult => {
            if (searchResult) {
              return searchResult.map(p =>
                new SearchProduct(p.Id, p.Name, this.checkExistInUserProducts(userProducts, p.Id), p.PicturePath,
                                  p.Composition, p.AvgRating));
            }
          }),
          catchError(
            this.handleError('Error in search.service getDefault()', [])
          )
        ).subscribe(p => {
          products.next(p);
        });
    });

    return products.asObservable();
  }

  getDefault(query: string) {
    const products: Subject<SearchProduct[]> = new Subject<SearchProduct[]>();
    const searchUrl = this.URL_SEARCH + '/products/default';

    this.httpService.get<SearchProduct[]>(searchUrl,  this.getOptions(query)).pipe(
      map(searchResult => {
        if (searchResult) {
          return searchResult.map(p =>
              new SearchProduct(p.Id, p.Name, false, p.PicturePath, p.Composition, p.AvgRating));
        }
      }),
      catchError(
        this.handleError('Error in search.service getDefault()', [])
      )
    ).subscribe(p => {
        products.next(p);
      });

    return products.asObservable();
  }

  getCustom(query: string, field: string) {
    const products: Subject<SearchProduct[]> = new Subject<SearchProduct[]>();
    const searchUrl = this.URL_SEARCH + '/products/custom';

    this.httpService.get<SearchProduct[]>(searchUrl, this.getOptionsCustom(query, field)).pipe(
      map(searchResult => {
        if (searchResult) {
          return searchResult.map(p =>
              new SearchProduct(p.Id, p.Name, false, p.PicturePath, p.Composition, p.AvgRating));
        }
      }),
      catchError(
        this.handleError('Error in search.service getDefault()', [])
      )
    ).subscribe(p => {
        products.next(p);
      });

    return products.asObservable();
  }

  getTopSearchQueries(query: string): Observable<UserQuery[]> {
    return this.httpService
      .get<UserQuery[]>(this.URL_SEARCH + '/queries', this.getOptions(query))
      .pipe(
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

  private getOptionsCustom(query: string, field: string) {
    return {
      params: new HttpParams().set('query', query + ';' + field)
    };
  }

  private checkExistInUserProducts(products: UserProduct[], id: string): boolean {
    return products.some(item => item.Product.Id === id );
  }
}
