import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { SearchService } from '../../services/service-search.service';
import { SearchProduct } from '../../models/SearchProduct.model';
import { Observable, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-component-search',
  templateUrl: './component-search.component.html',
  styleUrls: ['./component-search.component.css']
})
export class SearchComponent implements OnInit {
  MyQuery: 'sssss';
  private URL_SEARCH = 'http://localhost:2189/api/search/products';  // URL to web api
  showError = false;
  errorMessage: string;
  searchResult: SearchProduct[] = [];

  @Output() messageEvent = new EventEmitter<SearchProduct[]>();

  constructor(
    private http: HttpClient,
    private searchService: SearchService
    ) { }

  ngOnInit() {
  }

  search(query: string): void {
    if (typeof query !== 'undefined' && query) {
      // this.searchService.getDefault(query).subscribe(data => this.searchResult = data);
      this.searchResult[0] = new SearchProduct('iiii', 'nnnn', 'dddddd');
      this.messageEvent.emit(this.searchResult);
    }
  }

  runDefault(query: string): Observable<SearchProduct[]> {
    return this.http.get<SearchProduct[]>(this.URL_SEARCH + '/test?query=' + query).pipe(
      map(res => {
        const result: any = res;
        return result.map((item) => new SearchProduct(item.Id, item.Name, item.Description));
      }), catchError(this.handleError('Error response', []))
      );
  }

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
    /** Log a HeroService message with the MessageService */
    private log(message: string) {
      // this.messageService.add(`HeroService: ${message}`);
    }
}
