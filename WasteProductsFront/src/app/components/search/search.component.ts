import { Component, OnInit, Input } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SearchProduct } from '../../models/SearchProduct.model';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css']
})
export class SearchComponent implements OnInit {
  showError = false;
  errorMessage: string;

  searchResult: SearchProduct[] = [];
  // query: string;
  @Input() query: string;

  constructor(private searchService: SearchService) { }

  ngOnInit() {
  }

  ShowError(error: HttpErrorResponse) {
  }

  search(query: string): void {
    this.searchService.getDefault(query)
    .subscribe(data => this.searchResult = data);
  }

  getSearchProducts(): SearchProduct[] {
    return this.searchResult;
  }

}
