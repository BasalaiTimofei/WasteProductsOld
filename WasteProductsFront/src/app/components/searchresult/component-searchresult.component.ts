import { Component, OnInit, Input } from '@angular/core';
import { SearchProduct } from '../../models/SearchProduct.model';

@Component({
  selector: 'app-component-searchresult',
  templateUrl: './component-searchresult.component.html',
  styleUrls: ['./component-searchresult.component.css']
})
export class SearchresultComponent implements OnInit {
  searchProducts: SearchProduct[] = [];

  constructor() { }

  resultQuery: string;

  recieveMessage($event) {
    this.searchProducts = $event;
  }

  ngOnInit() {
  }
}
