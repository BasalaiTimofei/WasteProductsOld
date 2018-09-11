import { Component, OnInit } from '@angular/core';
import { SearchComponent } from '../search/search.component';
import { SearchProduct } from '../../models/SearchProduct.model';

@Component({
  selector: 'app-searchresult',
  templateUrl: './searchresult.component.html',
  styleUrls: ['./searchresult.component.css']
})
export class SearchresultComponent implements OnInit {
  searchProducts: SearchProduct[] = [];

  constructor( private searchComponent: SearchComponent ) { }

  ngOnInit() {
    this.getSearchProducts();
  }

  getSearchProducts() {
    this.searchProducts = this.searchComponent.getSearchProducts();
  }

}
