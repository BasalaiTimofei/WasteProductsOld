import { Component, OnInit } from '@angular/core';
import { User } from '../../../../models/users/user';
import { UserService } from '../../../../services/user/user.service';

@Component ({
  selector: 'app-userdata',
  templateUrl: './userdata.component.html',
  styleUrls: ['./userdata.component.css']
})
export class UserdataComponent implements OnInit {

  user: User = new User();
  isEmailChangingRequestSent: boolean;
  isUserNameChanged: boolean;

  constructor(public userService: UserService) { }

  ngOnInit() {
    this.isEmailChangingRequestSent = false;
    this.isUserNameChanged = null;
    this.userService.getUserSettings().subscribe(
      res => this.user = res,
      err => console.error(err));
    console.log(this.user);
  }

  updateUserName() {
    this.isUserNameChanged = null;
    this.userService.updateUserName(this.user.UserName)
    .subscribe(
      res => this.isUserNameChanged = true,
      err => this.isUserNameChanged = false
    );
  }

  updateEmail() {
    this.userService.updateEmailRequest(this.user.Email)
    .subscribe(
      res => this.isEmailChangingRequestSent = true,
      err => console.log(err)
    );
  }
}
