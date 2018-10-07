import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Login } from '../../models/login';
import { AuthenticationService } from '../../services/authentication.service';



@Component({
  selector: 'app-account-login',
  templateUrl: './account-login.component.html',
  styleUrls: ['./account-login.component.css']
})
export class AccountLoginComponent implements OnInit {

  model: Login = new Login('', '');
  errors: string;
  id: string = null;

  constructor(
    private authService: AuthenticationService
) { }


  ngOnInit() {
  }

  submitForm(form: NgForm) {
    this.authService.logIn(this.model);
  }

}
