import { Component, OnInit } from '@angular/core';
import { SearchService } from '../../services/search/search.service';
import { SearchProduct } from '../../models/search-product';
import { Observable, of } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { UserQuery } from '../../models/top-query';
import { Router } from '@angular/router';

import {FormControl} from '@angular/forms';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  query: string;
  topQueries: UserQuery[] = [];
  errorMessage: string;
  errorStatusCode: number;

  constructor(
    private searchService: SearchService,
    private router: Router
    ) { }

    myControl = new FormControl();
    filteredQueries: Observable<string[]>;

    ngOnInit() {
      // this.searchService.gettest().subscribe(console.log); // for test
    }

  search(query: string): void {
    if (typeof query !== 'undefined' && query) {
      this.query = '';
      this.router.navigate(['searchresults', query]);
      this.topQueries = [];
    }
  }

  searchInTopQueries(query: string): void {
    if (typeof query !== 'undefined' && query) {
      this.searchService.getTopSearchQueries(query).subscribe(
        data => this.topQueries = data.slice(0, 10),
                (err: HttpErrorResponse) => {
          this.errorMessage = 'Empty results...';
          if (err.status === 204) {
            this.errorStatusCode = err.status;
          }
        });
      }
  }

  clearQueries() {
    this.query = '';
    this.topQueries = [];
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
