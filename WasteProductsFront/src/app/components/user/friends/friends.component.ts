import { Component, OnInit } from '@angular/core';
import { FriendsService } from '../../../services/user/friends/friends.service';
import { User } from 'src/app/models/users/user';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  friends: User[];

  constructor(private srv: FriendsService) { }

  ngOnInit() {
    this.srv.loadFriends().subscribe(
    res => this.friends = res,
    err => console.error(err));
  }

}
