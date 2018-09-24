import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { SearchProduct } from '../../models/search-product';
import { UserQuery } from '../../models/top-query';
import { Observable, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { environment } from '../../../environments/environment.prod';
import { LoggingService } from '../logging/logging.service';


@Injectable({
  providedIn: 'root'
})
export class SearchService {
  searchProducts: SearchProduct[];
  private URL_SEARCH = `${environment.apiHostUrl}/api/search`;  // URL to web api

  constructor(
    private http: HttpClient,
    private logService: LoggingService ) { }

  getDefault(query: string): Observable<SearchProduct[]> {
    return this.http.get<SearchProduct[]>(this.URL_SEARCH + '/products/default', { params: new HttpParams().set('query', query)}).pipe(
      map(res => {
        const result: any = res;
        return result.map((item) => new SearchProduct(item.Id, item.Name));
      }), catchError(this.handleError('Error response', []))
      );
  }

  getTopSearchQueries(query: string): Observable<UserQuery[]> {
    return this.http.get<UserQuery[]>(this.URL_SEARCH + '/queries', { params: new HttpParams().set('query', query)}).pipe(
      map(res => {
        const result: any = res;
        return result.map(item => new UserQuery(item.QueryString));
      }), catchError(this.handleError('Error response', []))
      );
  }

  gettest(query: string) {
    return this.http.get('http://localhost:2189/api/search/products/default?query=' + query, {observe: 'response'});
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
    /** Log with the LoggingService */
  private log(msg: any) {
    this.logService.log(`SearchService: ${msg}`);
  }
}
