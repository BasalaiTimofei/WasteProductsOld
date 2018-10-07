import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { RegistrationModel } from '../../models/registration';
import { AuthenticationService } from '../../services/authentication.service';
import { UserService } from '../../../../services/user/user.service';


@Component({
  selector: 'app-account-register',
  templateUrl: './account-register.component.html',
  styleUrls: ['./account-register.component.css']
})
export class AccountRegisterComponent implements OnInit {
  registerFormGroup: FormGroup;
  confirmEmailGroup: FormGroup;

  @ViewChild('stepper') stepper;

  errors: string;
  registredUserid: string;

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthenticationService,
    private userService: UserService,
    private router: Router) { }

  ngOnInit() {
    this.registerFormGroup = this.formBuilder.group({
      UserName: ['',
        [
          Validators.required,
          Validators.minLength(6),
        ]
      ],
      Email: ['',
        [
          Validators.required,
          Validators.email
        ]
      ],
      Password: ['',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.minLength(12),
        ]
      ]
    });
    this.confirmEmailGroup = this.formBuilder.group({
      confirmEmailToken: ['',
        [
          Validators.required,
          Validators.pattern('[0-9]*'),
        ]
      ]
    });
  }
  //



  onSubmit() {
    const registrationModel: RegistrationModel = {
      UserName: this.registerFormGroup.controls['UserName'].value,
      Email: this.registerFormGroup.controls['Email'].value,
      Password: this.registerFormGroup.controls['Password'].value,
    };

    this.authService.register(registrationModel)
      .subscribe(
        userId => {
          if (userId) {
            this.registredUserid = userId;
            this.toNextStep();
          }
        });
  }

  onConfirmEmail() {
    if (this.registredUserid) {
      const token = this.confirmEmailGroup.controls['confirmEmailToken'].value;

      this.userService.confirmEmail(this.registredUserid, token)
        .subscribe(
          () => this.toNextStep(),
          () => this.errors = 'Неверный код подтверждения');
    }
  }

  goHomePage() {
    this.router.navigateByUrl('/');
  }

  private toNextStep() {
    this.stepper.next();
  }

}
