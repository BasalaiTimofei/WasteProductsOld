import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../../services/authentication.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {

    const profile = this.authService.getProfile();
    const claims = this.authService.getClaims();
    const inScope1 = this.authService.isInScope('wasteproducts-api');
    const inScope2 = this.authService.isInScope('nowasteproducts-api');


  }
}
