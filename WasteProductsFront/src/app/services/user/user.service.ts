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

  private getFriendsUrl = `${environment.apiHostUrl}/api/user/0/friends`;

  constructor(private httpClient: HttpClient, private authServise: AuthenticationService, loggingService: LoggingService) {
    super(httpClient, loggingService);
   }

  addFriend(friendId: string) {
    const claims = this.authServise.getClaims();
    const url = `${environment.apiHostUrl}/api/user/${claims.sub}/friends/${friendId}`;
    this.httpClient.put(url, null);
  }

  getFriends() {
    const claims = this.authServise.getClaims();
    const url = `${environment.apiHostUrl}/api/user/${claims.sub}/friends`;
    return this.httpClient.get<User[]>(url);
  }

  deleteFriend(friendId: string) {
    const claims = this.authServise.getClaims();
    const url = `${environment.apiHostUrl}/api/user/${claims.sub}/friends/${friendId}`;
    this.httpClient.delete(url);
  }
}
