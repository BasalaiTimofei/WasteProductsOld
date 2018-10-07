import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
/* Services */
import { BaseHttpService } from 'src/app/services/base/base-http.service';
import { LoggingService } from 'src/app/services/logging/logging.service';
/* Models */
import { GroupInfoModel, GroupModel, GroupOfUserModel } from '../models/group';
import { BoardInfoModel, BoardModel } from '../models/board';
import { ProductModel, ProductInfoModel } from '../models/product';

@Injectable({
  providedIn: 'root'
})
export class BoardService extends BaseHttpService {

  private apiUrl = `${environment.apiHostUrl}/api/groups`;

  constructor(httpClient: HttpClient, loggingService: LoggingService) {
    super(httpClient, loggingService);
  }

  createBoard(groupId, boardInfo: BoardInfoModel): Observable<BoardModel> {
    const url = `${this.apiUrl}/{groupId}/board`;

    return this.httpService.post<BoardModel>(url, boardInfo).pipe(
      tap(response => this.logDebug('creating board')),
      catchError(this.handleError('createBoard', null))
    );
  }

  updateBoard(boardId: string, boardInfo: BoardInfoModel): Observable<BoardModel> {
    const url = `${this.apiUrl}/board/${boardId}`;

    return this.httpService.put<BoardModel>(url, boardInfo).pipe(
      tap(response => this.logDebug('updating board')),
      catchError(this.handleError('updateBoard', null))
    );
  }

  deleteBoard(boardId: string): Observable<any> {
    const url = `${this.apiUrl}/board/${boardId}`;

    return this.httpService.delete(url).pipe(
      tap(response => this.logDebug('deleting board by group id')),
      catchError(this.handleError('deleteBoard')));
  }



  addProduct(boardId: string, productInfo: ProductInfoModel): Observable<ProductModel> {
    const url = `${this.apiUrl}/board/${boardId}/product`;

    return this.httpService.post<ProductModel>(url, productInfo).pipe(
      tap(response => this.logDebug('adding board to board')),
      catchError(this.handleError('addProduct', null))
    );
  }

  updateProduct(boardId: string, productInfo: ProductInfoModel): Observable<ProductModel> {
    const url = `${this.apiUrl}/board/${boardId}/product`;

    return this.httpService.post<ProductModel>(url, productInfo).pipe(
      tap(response => this.logDebug('adding board to board')),
      catchError(this.handleError('addProduct', null))
    );
  }

  deleteProduct(boardId: string, productId: string): Observable<any> {
    const url = `${this.apiUrl}/board/${boardId}/product/${productId}`;

    return this.httpService.delete(url).pipe(
      tap(response => this.logDebug('deleting product by product id')),
      catchError(this.handleError('deleteBoard')));
  }
}
