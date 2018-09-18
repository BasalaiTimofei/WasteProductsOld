import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/models/users/user';

@Injectable({
  providedIn: 'root'
})
export class FriendsModule {
  baseUrl = 'http:/localhost:2189/api/Currencies';

  constructor(private http: HttpClient) { }

  loadUsers() {
    return this.http.get<User[]>(this.baseUrl);
   }
 }
