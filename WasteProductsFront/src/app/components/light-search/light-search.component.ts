import { Component, OnInit, Output, EventEmitter } from '@angular/core';

import { SearchService } from '../../services/search/search.service';
import { BehaviorSubject, Observable } from 'rxjs';
import { SearchProduct } from '../../models/search-product';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-light-search',
  templateUrl: './light-search.component.html',
  styleUrls: ['./light-search.component.css']
})
export class LightSearchComponent implements OnInit {

  private tempResultSubject: BehaviorSubject<SearchProduct[]> = new BehaviorSubject<SearchProduct[]>([]);
  tempResult$: Observable<SearchProduct[]> = this.tempResultSubject.asObservable();
  @Output() productClickedEvent: EventEmitter<any> = new EventEmitter<any>();

  query: string;
  searchResult: SearchProduct[];
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  constructor(private searchService: SearchService) { }

  ngOnInit() {
  }

  search(query: string) {
    if (typeof query !== 'undefined' && query) {
      this.searchService.getCustom(query, 'Name').subscribe((r) => {
        this.searchResult = r;
        if (this.searchResult) {
          this.length = this.searchResult.length;
          this.changePageEvent();
        } else {
          this.length = 0;
        }
      });
    }
  }

  productClicked(product: any) {
    this.productClickedEvent.emit(product);
  }

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempResultSubject.next(this.searchResult.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1)));
  return event;
  }

}
