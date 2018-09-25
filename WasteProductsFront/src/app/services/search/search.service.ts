import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { SearchProduct } from '../../models/search-product';
import { UserQuery } from '../../models/top-query';
import { Observable} from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment.prod';
import { LoggingService } from '../logging/logging.service';
import { BaseHttpService } from '../base/base-http.service';


@Injectable({
  providedIn: 'root'
})
export class SearchService extends BaseHttpService {
  searchProducts: SearchProduct[];
  private URL_SEARCH = `${environment.apiHostUrl}/api/search`;  // URL to web api

  constructor(httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
   }

  getDefault(query: string): Observable<SearchProduct[]> {
    return this.httpService.get<SearchProduct[]>(this.URL_SEARCH + '/products/default', { params: new HttpParams().set('query', query)}).pipe(
      map(res => {
        const result: any = res;
        return result.map((item) => new SearchProduct(item.Id, item.Name));
      }), catchError(this.handleError('Error response', []))
      );
  }

  getTopSearchQueries(query: string): Observable<UserQuery[]> {
    return this.httpService.get<UserQuery[]>(this.URL_SEARCH + '/queries', { params: new HttpParams().set('query', query)}).pipe(
      map(res => {
        const result: any = res;
        return result.map(item => new UserQuery(item.QueryString));
      }), catchError(this.handleError('Error response', []))
      );
  }

  gettest(query: string) {
    return this.httpService.get('http://localhost:2189/api/search/products/default?query=' + query, {observe: 'response'});
  }
}
