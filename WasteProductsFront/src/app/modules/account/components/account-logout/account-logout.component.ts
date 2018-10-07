import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { Router } from '@angular/router';
import { AuthenticationService } from '../../services/authentication.service';


@Component({
  selector: 'app-account-logout',
  templateUrl: './account-logout.component.html',
  styleUrls: ['./account-logout.component.css']
})
export class AccountLogoutComponent implements OnInit {

  constructor(
    private authService: AuthenticationService,
    private location: Location,
    private router: Router) { }

  ngOnInit() {
  }

  goBack() {
    this.location.back();
  }

  logOut(event: MouseEvent) {
    this.authService.logOut();
    this.router.navigateByUrl('/');
  }



}
