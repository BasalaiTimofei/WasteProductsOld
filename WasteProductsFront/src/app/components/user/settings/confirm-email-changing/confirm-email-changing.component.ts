import { Component, OnInit } from '@angular/core';
import { UserService } from '../../../../services/user/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-confirm-email-changing',
  templateUrl: './confirm-email-changing.component.html',
  styleUrls: ['./confirm-email-changing.component.css']
})
export class ConfirmEmailChangingComponent implements OnInit {

  constructor(private service: UserService, private router: Router) {
    this.isConfirmed = true;
   }

  isConfirmed: boolean;

  ngOnInit() {
  }

  confirmNewEmail(token: string) {
    this.service.confirmEmailChanging(token)
    .subscribe
    (res => {
      this.isConfirmed = true;
      this.router.navigate(['/settings']);
    },
    err => this.isConfirmed = false);
  }
}
