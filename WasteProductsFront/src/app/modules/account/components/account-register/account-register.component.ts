import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Registration } from '../../models/registration';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../../../services/user/user.service';


@Component({
  selector: 'app-account-register',
  templateUrl: './account-register.component.html',
  styleUrls: ['./account-register.component.css']
})
export class AccountRegisterComponent implements OnInit {

  model: Registration = new Registration('', '', '');
  errors: string;
  isEmailConfirmationRequested = false;
  id: string = null;

  constructor(
    private authService: AuthenticationService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit() {
  }

  submitForm(form: NgForm) {
    this.authService.register(this.model)
    .subscribe(
      result => this.id = String(result),
      error => this.errors = error.error );

    this.isEmailConfirmationRequested = true;
  }

  confirmEmail(token: string) {
    this.userService.confirmEmail(this.id, token)
    .subscribe(
      result => {
        this.isEmailConfirmationRequested = false;
        this.router.navigate(['/']);
    }
    );
  }

}
