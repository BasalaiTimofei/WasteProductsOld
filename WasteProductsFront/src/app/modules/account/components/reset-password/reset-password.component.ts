import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../services/user/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})

export class ResetPasswordComponent implements OnInit {

  constructor(private service: UserService, private router: Router) {
    this.isRequestSent = false;
  }

  email: string;
  isRequestSent: boolean;
  userId: string;

  newPassword: string;
  token: string;

  errors: string;

  ngOnInit() {
  }

  submitForm(email: string) {
    this.isRequestSent = true;
    this.service.resetPasswordRequest(this.email)
    .subscribe(
      result => this.userId = String(result),
      error => this.errors = error.error );
  }

  changePassword() {
    this.service.resetPassword(this.userId, this.token, this.newPassword)
    .subscribe(
      res =>  this.router.navigate(['/']),
      error => this.errors = error.error
    );
  }
}
