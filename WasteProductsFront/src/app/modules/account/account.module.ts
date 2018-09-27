import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { MaterialModule } from 'src/app/modules/material/material.module';

/* Components */
import { AccountPanelComponent } from './components/account-panel/account-panel.component';
import { AccountPanelButtonComponent } from './components/account-panel-button/account-panel-button.component';
import { AccountRegisterComponent } from './components/account-register/account-register.component';
import { AccountComponent } from './components/account/account.component';

// Environment
import { environment } from 'src/environments/environment.prod';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    FormsModule,

    /* Auth module*/
    OAuthModule.forRoot({
      resourceServer: {
        allowedUrls: [environment.apiHostUrl],
        sendAccessToken: true
      }
    }),

    /* Material module */
    MaterialModule
  ],
  declarations: [
    AccountPanelComponent,
    AccountPanelButtonComponent,
    AccountRegisterComponent,
    AccountComponent
  ],
  exports: [
    OAuthModule,

    AccountPanelButtonComponent,
    AccountRegisterComponent,
    AccountComponent
  ],
  entryComponents: [
    AccountPanelComponent
  ]
})
export class AccountModule { }
