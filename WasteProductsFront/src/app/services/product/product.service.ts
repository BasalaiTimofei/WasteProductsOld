import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { BaseHttpService } from '../base/base-http.service';
import { LoggingService } from '../logging/logging.service';

// environment
import { environment } from '../../../environments/environment.prod';
import { UserProduct } from '../../models/users/user-product';

@Injectable({
  providedIn: 'root'
})
export class ProductService extends BaseHttpService {

  private apiUrl = `${environment.apiHostUrl}/api/product/products`;

  private getProductsUrl = `http://localhost:2189/api/user/0/products`;

  constructor(httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
  }

  getProductSet(): any {
    throw new Error('Method not implemented.'); // change to correct method
  }

  loadUserProducts() {
    return this.httpService.get<UserProduct[]>(this.getProductsUrl);
   }
}
