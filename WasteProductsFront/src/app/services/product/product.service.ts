import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  [x: string]: any;

  private apiUrl = `${environment.apiHostUrl}/api/product/products`;

  constructor() { }
}
