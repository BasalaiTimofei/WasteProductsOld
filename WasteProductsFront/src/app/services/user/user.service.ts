import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { User } from 'src/app/models/users/user';
import { environment } from '../../../environments/environment';
import { LoggingService } from '../logging/logging.service';
import { AuthenticationService } from '../../modules/account/services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService  {
  constructor(httpClient: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpClient, loggingService);
    this.userApiUrl = `${environment.apiHostUrl}/api/user/`;
   }

   private userApiUrl;

  addFriend(friendId: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/friends/${friendId}`;
    this.httpService.put(url, null);
  }

  getFriends() {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/friends`;
    return this.httpService.get<User[]>(url);
  }

  deleteFriend(friendId: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/friends/${friendId}`;
    this.httpService.delete(url);
  }

  getUserSettings() {
    return this.httpService.get<User>(`this.userApiUrl/${this.authServise.getUserId()}`);
  }

  updateUserName(userName: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/updateusername`;
    const bodyObj = {
      UserName: userName
    };
    return this.httpService.put(url, bodyObj);
  }

  updateEmail(email: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/updateemail`;
    const bodyObj = {
      EmailOfTheUser: email,
    };
    return this.httpService.put(url, bodyObj);
  }

  confirmEmailChanging(token: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/confirmemailchanging/${token}`;
    return this.httpService.put(url, null);
  }

  updatePassword(oldPassword: string, newPassword: string) {
    const url = `${this.userApiUrl}/${this.authServise.getUserId()}/changepassword`;
    const bodyObj = {
      OldPassword: oldPassword,
      NewPassword: newPassword
    };
    return this.httpService.put(url, bodyObj);
  }
}
