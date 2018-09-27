import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/models/users/user';

@Injectable({
  providedIn: 'root'
})
export class FriendsService {
  getFriendsUrl = `http://localhost:2189/api/user/0/friends`;

  constructor(private http: HttpClient) { }

  loadFriends() {
    return this.http.get<User[]>(this.getFriendsUrl);
   }
}
