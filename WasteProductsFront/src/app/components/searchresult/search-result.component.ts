import { Component, OnInit, Input } from '@angular/core';
import { SearchProduct } from '../../models/SearchProduct.model';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnInit {
  searchProducts: SearchProduct[] = [];
  statusCode: 200;

  constructor() { }

  resultQuery: string;

  recieveSearchResult($event) {
    this.searchProducts = $event;
  }

  recieveStatusCode($event) {
    this.statusCode = $event;
  }

  ngOnInit() {
  }
}
