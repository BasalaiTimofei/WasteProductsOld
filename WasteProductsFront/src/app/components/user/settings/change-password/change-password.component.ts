import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../services/user/user.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  constructor(private service: UserService, private router: Router) { }

  ngOnInit() {
  }

  changePassword(oldPassword: string, newPassword: string, newPasswordConfirm: string) {
    if (newPassword === newPasswordConfirm) {
      this.service.updatePassword(oldPassword, newPassword)
      .subscribe(res => {
        this.router.navigate(['/settings']);
      });
    }
  }
}
