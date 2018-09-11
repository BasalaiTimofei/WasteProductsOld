import { Component, OnInit } from '@angular/core';
import { SearchProduct } from '../../models/SearchProduct.model';
import { SearchService } from '../../services/search.service';

@Component({
  selector: 'app-searchresult',
  templateUrl: './searchresult.component.html',
  styleUrls: ['./searchresult.component.css']
})
export class SearchresultComponent implements OnInit {
  searchProducts: SearchProduct[] = [];
  query: string;

  constructor( private searchService: SearchService ) { }

  ngOnInit() {
    this.getSearchProductsFromService();
  }

  getSearchProductsFromService() {
    // this.searchProducts = this.searchService.getSearchProducts();
  }

}
