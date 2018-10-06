import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BaseHttpService } from '../base/base-http.service';
import { User } from 'src/app/models/users/user';
import { environment } from '../../../environments/environment';
import { LoggingService } from '../logging/logging.service';
import { AuthenticationService } from '../../modules/account/services/authentication.service';
import { Email } from '../../modules/account/models/email';
import { ResetPasswordResult } from '../../modules/account/models/reset-password-result';

@Injectable({
  providedIn: 'root'
})
export class UserService extends BaseHttpService  {
  constructor(httpClient: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpClient, loggingService);
    this.baseUserApiUrl = `${environment.apiHostUrl}/api/user`;
   }

   private baseUserApiUrl;

  addFriend(friendId: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/friends/${friendId}`;
    this.httpService.put(url, null);
  }

  getFriends() {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/friends`;
    return this.httpService.get<User[]>(url);
  }

  deleteFriend(friendId: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/friends/${friendId}`;
    this.httpService.delete(url);
  }

  getUserSettings() {
    return this.httpService.get<User>(`${this.baseUserApiUrl}/${this.authServise.getUserId()}`);
  }

  updateUserName(userName: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/updateusername`;
    const bodyObj = {
      UserName: userName
    };
    return this.httpService.put(url, bodyObj);
  }

  updateEmailRequest(email: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/updateemail`;
    const bodyObj = {
      EmailOfTheUser: email
    };
    return this.httpService.put(url, bodyObj);
  }

  confirmEmail(id: string, token: string) {
    const url = `${this.baseUserApiUrl}/${id}/confirmemail/${token}`;
    return this.httpService.put(url, null);
  }

  confirmEmailChanging(token: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/confirmemailchanging/${token}`;
    return this.httpService.put(url, null);
  }

  updatePassword(oldPassword: string, newPassword: string) {
    const url = `${this.baseUserApiUrl}/${this.authServise.getUserId()}/changepassword`;
    const bodyObj = {
      OldPassword: oldPassword,
      NewPassword: newPassword
    };
    return this.httpService.put(url, bodyObj);
  }

  resetPasswordRequest(email: string) {
    const url = `${this.baseUserApiUrl}/resetpasswordrequest`;
    const bodyObj = {
      EmailOfTheUser: email
    };
    return this.httpService.put(url, bodyObj);
  }

  resetPassword(resetResult: ResetPasswordResult, newPassword: string) {
    const url = `${this.baseUserApiUrl}/${resetResult.UserId}/resetpasswordresponse/${resetResult.Token}`;
    const bodyObj = {
      Password: newPassword
    };
    return this.httpService.put(url, bodyObj);
  }
}
