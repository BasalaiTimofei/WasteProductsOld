import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/models/users/user';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private getFriendsUrl = `${environment.apiHostUrl}/api/user/0/friends`;

  constructor(private httpClient: HttpClient) { }

  loadFriends() {
    return this.httpClient.get<User[]>(this.getFriendsUrl);
  }
}
