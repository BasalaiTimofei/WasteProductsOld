import { Component, OnInit, Input } from '@angular/core';
import { SearchProduct } from '../../models/search-product';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnInit {
  @Input() searchProducts: SearchProduct[];
  @Input() statusCode: number;

  constructor() { }

  ngOnInit() {
  }
}
