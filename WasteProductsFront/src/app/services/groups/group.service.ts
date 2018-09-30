import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GroupOfUser } from 'src/app/models/users/group-of-user';

@Injectable({
  providedIn: 'root'
})
export class GroupService {
  getGroupsOfUserUrl = `http://localhost:2189/api/user/0/groups`;

  constructor(private http: HttpClient) { }

  loadGroupsOfUser() {
    return this.http.get<GroupOfUser[]>(this.getGroupsOfUserUrl);
   }
}
