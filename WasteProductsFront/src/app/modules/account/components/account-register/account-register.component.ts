import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgForm } from '@angular/forms';
import { Registration } from '../../models/registration';
import { AuthenticationService } from '../../services/authentication.service';


@Component({
  selector: 'app-account-register',
  templateUrl: './account-register.component.html',
  styleUrls: ['./account-register.component.css']
})
export class AccountRegisterComponent implements OnInit {

  model: Registration = new Registration('', '', '');
  errors: string;


  constructor(
    private authService: AuthenticationService,
    private router: Router) { }

  ngOnInit() {
  }

  submitForm(form: NgForm) {
    this.authService.register(this.model).subscribe(succes => {
      this.router.navigate(['/']);
    }, error => {
      this.errors = error.error;
    });
  }

}
