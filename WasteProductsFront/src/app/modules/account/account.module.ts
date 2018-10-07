import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OAuthModule } from 'angular-oauth2-oidc';
import { SignalRModule, SignalRConfiguration } from 'ng2-signalr';
import { MaterialModule } from 'src/app/modules/material/material.module';
/* Components */
import { AccountPanelComponent } from './components/account-panel/account-panel.component';
import { AccountPanelButtonComponent } from './components/account-panel-button/account-panel-button.component';
import { AccountRegisterComponent } from './components/account-register/account-register.component';
import { AccountComponent } from './components/account/account.component';
import { NotificationListComponent } from './components/notification-list/notification-list.component';
import { NotificationDetailsComponent } from './components/notification-details/notification-details.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { AccountLoginComponent } from './components/account-login/account-login.component';
/* Pipes */
import { TruncatePipe } from './pipes/truncate.pipe';
// Environment
import { environment } from 'src/environments/environment';


function createConfig(): SignalRConfiguration {
  const config = new SignalRConfiguration();
  config.logging = !environment.production;
  return config;
}

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
    /* SignalR */
    SignalRModule.forRoot(createConfig),
    /* Material module */
    MaterialModule
  ],
  declarations: [
    /* Pipes */
    TruncatePipe,
    /* Components */
    AccountPanelComponent,
    AccountPanelButtonComponent,
    AccountRegisterComponent,
    AccountComponent,
    NotificationListComponent,
    NotificationDetailsComponent,
    ResetPasswordComponent,
    AccountLoginComponent,
  ],
  exports: [
    OAuthModule, // TODO: check
    /* Pipes */
    TruncatePipe,
    /* Components */
    AccountPanelButtonComponent,
    AccountRegisterComponent,
    AccountComponent,
    NotificationListComponent,
    NotificationDetailsComponent,
    AccountLoginComponent
  ],
  entryComponents: [
    AccountPanelComponent
  ]
})
export class AccountModule { }
