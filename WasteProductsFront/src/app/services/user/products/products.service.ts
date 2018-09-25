
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ProductDescription } from '../../../models/users/product-description';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = `http://localhost:2189/api/user/0/products`;

  constructor(private http: HttpClient) { }

  loadProducts() {
    return this.http.get<ProductDescription[]>(this.baseUrl);
   }
}
