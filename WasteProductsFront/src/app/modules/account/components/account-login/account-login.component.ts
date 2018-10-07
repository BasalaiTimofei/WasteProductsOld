import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { LoginModel } from '../../models/login';
import { AuthenticationService } from '../../services/authentication.service';




@Component({
  selector: 'app-account-login',
  templateUrl: './account-login.component.html',
  styleUrls: ['./account-login.component.css']
})
export class AccountLoginComponent implements OnInit {

  model: LoginModel = new LoginModel('', '');
  errors: string;



  constructor(
    private authService: AuthenticationService,
    private router: Router) { }


  ngOnInit() {

  }

  submitForm(form: NgForm) {
    this.authService.logInResourceOwnerFlow(this.model).subscribe(isLogined => {
      if (isLogined) {
        const claims = this.authService.getClaims();

        if (claims.email_verified === 'true') {
          this.router.navigateByUrl('/');
        } else {
          this.authService.logOut();
          this.errors = 'Email не подтверждён';
        }
      } else {
        this.errors = 'Неверное имя пользователя или пароль';
      }
    });
  }

}
