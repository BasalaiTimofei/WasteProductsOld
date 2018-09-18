import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { SearchProduct } from '../../models/search-product';
import { MatPaginator, MatTableDataSource } from '@angular/material';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnInit {
  @Input() searchProducts: SearchProduct[];
  @Input() statusCode: number;
  tempProducts: SearchProduct[] = [];
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  ngOnInit() {
    this.length = this.searchProducts.length;
    this.changePageEvent();
  }

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempProducts = this.searchProducts.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1));
  return event;
  }

  constructor() {
   }
}
