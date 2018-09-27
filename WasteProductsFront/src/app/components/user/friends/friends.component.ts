import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/users/user';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  friends: User[];

  constructor(private srv: UserService) { }

  ngOnInit() {
    this.srv.loadFriends().subscribe(
    res => this.friends = res,
    err => console.error(err));
  }

}
