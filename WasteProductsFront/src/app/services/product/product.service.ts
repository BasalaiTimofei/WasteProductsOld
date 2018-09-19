import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  getProductSet(): any {
    throw new Error("Method not implemented."); //change to correct method
  }

  private apiUrl = `${environment.apiHostUrl}/api/product/products`;

  constructor() { }
}
