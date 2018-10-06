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
import { BoardInfoModel } from '../models/board';

@Injectable({
  providedIn: 'root'
})
export class BoardService extends BaseHttpService {

  private apiUrl = `${environment.apiHostUrl}/api/groups`;

  constructor(httpClient: HttpClient, loggingService: LoggingService) {
    super(httpClient, loggingService);
  }

  createBoard(groupId, boardInfo: BoardInfoModel) {

  }

  deleteBoard(boardId: string): Observable<any> {
    const url = `${this.apiUrl}/board/${boardId}`;
    return this.httpService.delete(url).pipe(
      tap(response => this.logDebug('deleting board by group id')),
      catchError(this.handleError('deleteBoard')));
  }

}
