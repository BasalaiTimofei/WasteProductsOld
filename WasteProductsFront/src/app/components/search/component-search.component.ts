import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { SearchService } from '../../services/search/service-search.service';
import { SearchProduct } from '../../models/SearchProduct.model';
import { Observable, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-component-search',
  templateUrl: './component-search.component.html',
  styleUrls: ['./component-search.component.css']
})
export class SearchComponent implements OnInit {
  query: string;
  query2: string;
  private URL_SEARCH = 'http://localhost:2189/api/search/products';  // URL to web api
  showError = false;
  errorMessage: string;
  errorStatusCode: number;
  searchResult: SearchProduct[] = [];

  @Output() searchCollectionEvent = new EventEmitter<SearchProduct[]>();
  @Output() statusCodeEvent = new EventEmitter<number>();

  constructor(
    private http: HttpClient,
    private searchService: SearchService
    ) { }

  ngOnInit() {
  }

  search(query: string): void {
    if (typeof query !== 'undefined' && query) {
      /*this.searchService.getDefault(query).subscribe(
        data => this.searchResult = data
        , (err: HttpErrorResponse) => {
          this.errorMessage = 'Empty results...';
          if (err.status === 204) {
            this.errorStatusCode = err.status;
          }
        });*/
      // this.searchResult[0] = new SearchProduct('iiii', 'nnnn', 'dddddd');
      this.searchCollectionEvent.emit(this.searchResult);
      this.statusCodeEvent.emit(200);
    }
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
