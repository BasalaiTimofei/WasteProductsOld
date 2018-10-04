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

  getUserGroup(userId: string): Observable<GroupModel> {
    const url = `${environment.apiHostUrl}/api/user/${userId}/group`;
    return this.httpService.get<GroupModel>(url).pipe(
      tap(response => this.logDebug('fetched own group by user id')),
      catchError(this.handleError('getUserGroup', new GroupModel()))
    );
  }

  getUserOtherGroups(userId: string): Observable<GroupOfUserModel[]> {
    const url = `${environment.apiHostUrl}/api/user/${userId}/groups`;
    return this.httpService.get<GroupOfUserModel[]>(url).pipe(
      tap(response => this.logDebug('fetched other group by user id')),
      catchError(this.handleError('getUserOtherGroups', []))
    );
  }

  crateGroup(userId: string, groupInfo: GroupInfoModel): Observable<GroupModel> {
    const data = Object.assign(new GroupModel, groupInfo);
    data.AdminId = userId;

    return this.httpService.put<GroupModel>(this.apiUrl, groupInfo).pipe(
      tap(response => this.logDebug('creating group')),
      catchError(this.handleError('crateGroup', new GroupModel()))
    );
  }

  getGroup(groupId: string): Observable<GroupModel> {
    const url = `${this.apiUrl}/${groupId}`;
    return this.httpService.get<GroupModel>(url).pipe(
      tap(response => this.logDebug('fetched group by group id')),
      catchError(this.handleError('getGroup', new GroupModel()))
    );
  }

  updateInfo(groupId: string, groupInfo: GroupInfoModel): Observable<GroupModel> {
    const url = `${this.apiUrl}/${groupId}`;

    return this.httpService.put<GroupModel>(url, groupInfo).pipe(
      tap(response => this.logDebug('updating group')),
      catchError(this.handleError('getGroup', new GroupModel()))
    );
  }
}
