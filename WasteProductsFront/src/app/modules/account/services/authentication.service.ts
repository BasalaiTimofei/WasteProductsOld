import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
/* Services */
import { BaseHttpService } from 'src/app/services/base/base-http.service';
import { LoggingService } from 'src/app/services/logging/logging.service';
import { OAuthService, JwksValidationHandler, AuthConfig } from 'angular-oauth2-oidc';
/* Environment */
import { environment } from 'src/environments/environment';

import { RegistrationModel } from '../models/registration';
import { LoginModel } from '../models/login';



declare interface Claims {
  /**
     * Subject / Id
     */
  sub: string;
  /**
     * Email
     */
  email: string;
  /**
     * Is email verified
     */
  email_verified: string;
  /**
     * Full name
     */
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseHttpService {

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);

  isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private oauthService: OAuthService, httpService: HttpClient, loggingService: LoggingService) {
    super(httpService, loggingService);
    this.configureOauth();
  }

  register(registrationModel: RegistrationModel): Observable<any> {
    const url = `${environment.apiHostUrl}/api/user/register`;  // URL to web api
    return this.httpService.post<any>(url, registrationModel).pipe(
      tap(data => this.logDebug('registering new user')),
      catchError(this.handleError('getState', null))
    );
  }

  logInResourceOwnerFlow(userModel: LoginModel): Observable<boolean> {
    const result: Subject<boolean> = new Subject<boolean>();

    this.oauthService.fetchTokenUsingPasswordFlowAndLoadUserProfile(userModel.UserName, userModel.Password)
      .then(() => {
        result.next(true);
        this.logDebug('LogIn completed');
      })
      .catch(() => {
        result.next(false);
        this.logError('LogIn error');
      });

    return result.asObservable();
  }

  logInImplicitFlow() {
    this.oauthService.initImplicitFlow();
  }

  logOut(): void {
    this.oauthService.logOut();
  }

  getProfile(): Promise<object> {
    return this.oauthService.loadUserProfile();
  }

  getAccessToken(): string {
    return this.oauthService.getAccessToken();
  }

  getClaims(): Claims {
    return <Claims>this.oauthService.getIdentityClaims();
  }

  getUserId(): string {
    const claims = this.getClaims();
    if (claims) { return this.getClaims().sub; }
    return null;
  }

  isInScope(scope: string): boolean {
    const scopesString = (<Array<string>>this.oauthService.getGrantedScopes())[0];
    const scopes = scopesString.split(' ');
    return scopes.includes(scope);
  }

  private configureOauth() {
    // set storage for tokens
    this.oauthService.setStorage(sessionStorage);
    // set AuthConfig
    this.oauthService.configure(this.createAuthConfig());
    // set automatic refresh
    this.oauthService.setupAutomaticSilentRefresh();
    // set token validation handler
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    // Load Discovery Document and then try to login the user
    this.oauthService.loadDiscoveryDocument()
      .then(() => {
        this.logDebug('Connection to the IdentityServer established and Discovery Document was successfully loaded');
        // try to login
        this.oauthService.tryLogin({
          onTokenReceived: () =>
            this.logDebug('LogIn completed'),
          onLoginError: () =>
            this.logError('LogIn error')
        });
      }).catch(() => this.logError('The AuthenticationService can not connect to the IdentityServer or load DiscoveryDocument from it'));

    // subscribe to login/logout events
    this.oauthService.events.subscribe(event => {
      const oldValue = this.isAuthenticatedSubject.value;
      const newValue = this.oauthService.hasValidAccessToken();
      if (oldValue !== newValue) {
        this.isAuthenticatedSubject.next(newValue);
      }
    });

  }

  private createAuthConfig(): AuthConfig {
    return {
      oidc: false,
      issuer: environment.iderntityHostUrl, // Url of the Identity Provider
      scope: 'openid profile email wasteproducts-api', // set the scope for the permissions the client should request

      clientId: environment.clientId, // The SPA's id. The SPA is registerd with this id at the auth-server
      dummyClientSecret: environment.dummyClientSecret,

      showDebugInformation: !environment.production,
      // sessionChecksEnabled: true,
    };
  }
}
