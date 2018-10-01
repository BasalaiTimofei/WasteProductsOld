import { Component, OnInit } from '@angular/core';
import {FormControl, Validators} from '@angular/forms';
import {UserService} from '../../../../services/user/user.service';
import { User } from '../../../../models/users/user';

/** @title Form field with error messages */
@Component({
  selector: 'app-form-field-error',
  templateUrl: './userformfield.component.html',
  styleUrls: ['./userformfield.component.css'],
})
export class UserformfieldComponent {
  srv: UserService;
  user: User;
  userName = new FormControl('', [Validators.minLength(4), Validators.maxLength(16)]);

  getErrorMessage() {
    return this.userName.hasError('minLength(4)') ? 'Минимальная длина имени - 4 символа' :
        this.userName.hasError('maxLength(16)') ? 'Максимальная длина имени - 16 символов' :
            '';
  }
}
