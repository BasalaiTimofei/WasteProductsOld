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



@Injectable({
  providedIn: 'root'
})
export class GroupsService extends BaseHttpService {


  private apiUrl = `${environment.apiHostUrl}/api/groups`;

  constructor(httpClient: HttpClient, loggingService: LoggingService) {
    super(httpClient, loggingService);
  }

  getUserGroups(userId: string): Observable<GroupOfUserModel[]> {
    const url = `${environment.apiHostUrl}/api/user/${userId}/groups`;
    return this.httpService.get<GroupOfUserModel[]>(url).pipe(
      tap(response => this.logDebug('fetched other group by user id')),
      catchError(this.handleError('getUserOtherGroups', []))
    );
  }

  getGroup(groupId: string): Observable<GroupModel> {
    const url = `${this.apiUrl}/${groupId}`;

    return this.httpService.get<GroupModel>(url).pipe(
      tap(response => this.logDebug('fetched group by group id')),
      catchError(this.handleError('getGroup', new GroupModel()))
    );
  }

  createGroup(userId: string, groupInfo: GroupInfoModel): Observable<GroupModel> {
    return this.httpService.post<GroupModel>(this.apiUrl, groupInfo).pipe(
      tap(response => this.logDebug('creating group')),
      catchError(this.handleError('crateGroup', new GroupModel()))
    );
  }

  updateGroup(groupId: string, groupInfo: GroupInfoModel): Observable<any> {
    const url = `${this.apiUrl}/${groupId}`;

    return this.httpService.put<GroupModel>(url, groupInfo).pipe(
      tap(response => this.logDebug('updating group')),
      catchError(this.handleError('getGroup'))
    );
  }

  deleteGroup(groupId: string): Observable<any> {
    const url = `${this.apiUrl}/${groupId}`;

    return this.httpService.delete(url).pipe(
      tap(response => this.logDebug('deleting group by group id')),
      catchError(this.handleError('deleteGroup'))
    );
  }


  comeIntoGroup(groupId: string, userId): Observable<any> {
    const url = `${this.apiUrl}/${groupId}/invite/${userId}`;
    return this.httpService.post(url, {}).pipe(
      tap(response => this.logDebug('comming into group')),
      catchError(this.handleError('comeIntoGroup'))
    );

  }

  leftGroup(groupId: string, userId: string): Observable<any> {
    const url = `${this.apiUrl}/${groupId}/kick/${userId}`;

    return this.httpService.delete(url).pipe(
      tap(response => this.logDebug('lefting group')),
      catchError(this.handleError('leftGroup'))
    );
  }
}
