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
  constructor(private httpClient: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpClient, loggingService);
    this.apiUrlPlusUserId = `${environment.apiHostUrl}/api/user/${this.authServise.getUserId()}`;
   }

   private apiUrlPlusUserId;

  addFriend(friendId: string) {
    const url = `${this.apiUrlPlusUserId}/friends/${friendId}`;
    this.httpClient.put(url, null);
  }

  getFriends() {
    const url = `${this.apiUrlPlusUserId}/friends`;
    return this.httpClient.get<User[]>(url);
  }

  deleteFriend(friendId: string) {
    const url = `${this.apiUrlPlusUserId}/friends/${friendId}`;
    this.httpClient.delete(url);
  }

  getUserSettings() {
    return this.httpClient.get<User>(this.apiUrlPlusUserId);
  }

  updateUserName(userName: string) {
    const url = `${this.apiUrlPlusUserId}/updateusername`;
    const bodyObj = {
      UserName: userName
    };
    return this.httpClient.put(url, bodyObj);
  }

  updateEmail(email: string)  {
    const url = `${this.apiUrlPlusUserId}/updateemail`;
    const bodyObj = {
      EmailOfTheUser: email,
    };
    return this.httpClient.put(url, bodyObj);
  }

  updatePassword(oldPassword: string, newPassword: string) {
    const url = `${this.apiUrlPlusUserId}/changepassword`;
    const bodyObj = {
      OldPassword: oldPassword,
      NewPassword: newPassword
    };
    return this.httpClient.put(url, bodyObj);
  }
}
