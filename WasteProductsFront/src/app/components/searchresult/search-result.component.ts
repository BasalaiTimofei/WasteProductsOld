import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { SearchProduct } from '../../models/search-product';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { PageEvent } from '@angular/material';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { SearchService } from '../../services/search/search.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnInit {
  query: string;
  searchResult: SearchProduct[];
  statusCode: number;
  tempProducts: SearchProduct[];
  errorMessage: string;
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  /*ngOnInit() {
    if (this.searchProducts && typeof this.searchProducts !== 'undefined') {

      this.length = this.searchProducts.length;
      this.changePageEvent();
    }
  }*/

  ngOnInit() {
    this.query = this.route.snapshot.paramMap.get('query');
    if (typeof this.query !== 'undefined' && this.query) {
      this.search(this.query);
      this.searchService.gettest(this.query).subscribe(console.log);
    }
  }

  constructor(
    private http: HttpClient,
    private searchService: SearchService,
    private route: ActivatedRoute) {
   }

  search(query: string): void {
      this.searchService.getDefault(query).subscribe(
        data => this.searchResult = data
        , (err: HttpErrorResponse) => {
          this.errorMessage = 'Empty results...';
          if (err.status === 204) {
            this.statusCode = err.status;
          }
        });
        if (this.searchResult) {
          this.length = this.searchResult.length;
          this.changePageEvent();
        }
  }

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempProducts = this.searchResult.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1));
  return event;
  }
}
