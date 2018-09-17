import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { SearchService } from '../../services/search/search.service';
import { SearchProduct } from '../../models/SearchProduct.model';
import { Observable, of } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';

import {FormControl} from '@angular/forms';
import {startWith} from 'rxjs/operators';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  query: string;
  topQueries: string[] = [];
  selectedQuery: string;
  private URL_SEARCH = 'http://localhost:2189/api/search/products';  // URL to web api
  showError = false;
  errorMessage: string;
  errorStatusCode: number;
  searchResult: SearchProduct[] = [];

  constructor(
    private http: HttpClient,
    private searchService: SearchService
    ) { }

    myControl = new FormControl();
    filteredQueries: Observable<string[]>;

    ngOnInit() {
      this.topQueries[0] = '1q111';
      this.topQueries[1] = '2222';
      this.topQueries[2] = '3333';
      this.topQueries[3] = '4444';
      this.topQueries[4] = '5555';
      // this.searchInTopQueries('');
      this.filteredQueries = this.myControl.valueChanges.pipe(
        startWith(''),
        map(value => this._filter(value))
      );
    }

    private _filter(value: string): string[] {
      const filterValue = value.toLowerCase();
      return this.topQueries.filter(option => option.toLowerCase().indexOf(filterValue) === 0);
    }

  search(query: string): void {
    if (typeof query !== 'undefined' && query) {
      this.topQueries.length = 0;
      /*this.searchService.getDefault(query).subscribe(
        data => this.searchResult = data
        , (err: HttpErrorResponse) => {
          this.errorMessage = 'Empty results...';
          if (err.status === 204) {
            this.errorStatusCode = err.status;
          }
        });*/
      this.searchResult[0] = new SearchProduct('iiii', 'nnnn', 'dddddd');
    }
  }

  searchInTopQueries(query: string): void {
      /*this.searchService.getTopSearchQueries(query).subscribe(
        data => this.topQueries = data
        , (err: HttpErrorResponse) => {
          this.errorMessage = 'Empty results...';
          if (err.status === 204) {
            this.errorStatusCode = err.status;
          }
        });*/
      this.topQueries[0] = 'qwe rty';
      this.topQueries[1] = '1qwe zxc';
      this.topQueries[2] = 'zzsd';
      this.topQueries[3] = 'zzsw';
      this.topQueries[4] = 'qwerty';
      for (let i = 5; i < 10; i++) {
        this.topQueries[i] = 'Top query ' + i;
    }
  }

  clearQueries() {
    this.query = '';
    this.topQueries.length = 0;
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
