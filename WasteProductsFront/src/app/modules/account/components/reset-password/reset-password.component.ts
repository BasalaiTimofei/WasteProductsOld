import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../services/user/user.service';
import { ResetPasswordResult } from '../../models/reset-password-result';
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

  newPassword: string;

  errors: string;

  isRequestSent: boolean;

  requestResult: ResetPasswordResult;

  ngOnInit() {
  }

  submitForm(email: string) {
    this.service.resetPasswordRequest(this.email)
    .subscribe(
      result => {
        this.isRequestSent = true;

      },
      error => this.errors = error.error );
  }

  changePassword() {
    this.service.resetPassword(this.requestResult, this.newPassword)
    .subscribe(
      res => {
        this.router.navigate(['/']);
      }
    );
  }
}
