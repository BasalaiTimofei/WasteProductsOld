import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

/* Services */
import { BaseService } from 'src/app/services/base/base.service';
import { LoggingService } from 'src/app/services/logging/logging.service';
import { OAuthService, JwksValidationHandler, AuthConfig } from 'angular-oauth2-oidc';

// Environment
import { environment } from '../../../../environments/environment';

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
  email_varified: string;
  /**
     * Full name
     */
  name: string;
  /**
     * Given name
     */
  given_name: string;
  /**
     * Family name
     */
  family_name: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService extends BaseService {

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(false);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  public constructor(private oauthService: OAuthService, loggingService: LoggingService) {
    super(loggingService);
    this.configureOauth();
  }

  public logIn(): void {
    this.oauthService.initImplicitFlow();
  }

  public logOut(): void {
    this.oauthService.logOut();
  }

  public getClaims(): Claims {
    return <Claims>this.oauthService.getIdentityClaims();
  }

  public getProfile(): object {
    return this.oauthService.loadUserProfile();
  }

  public isInScope(scope: string): boolean {
    const scopesString = (<Array<string>>this.oauthService.getGrantedScopes())[0];
    const scopes = scopesString.split(' ');
    return scopes.includes(scope);
  }

  private configureOauth() {
    // set storage for tokens
    this.oauthService.setStorage(sessionStorage);
    // set AuthConfig
    this.oauthService.configure(this.getAuthConfig());
    // set automatic refresh
    this.oauthService.setupAutomaticSilentRefresh();
    // set token validation handler
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    // subscribe to login/logout events
    this.oauthService.events.subscribe(event =>
      this.isAuthenticatedSubject.next(this.oauthService.hasValidIdToken() && this.oauthService.hasValidAccessToken())
    );

    // trying to load DiscoveryDocument
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
      })
      .catch(() => this.logError('The AuthenticationService can not connect to the IdentityServer or load DiscoveryDocument from it'));
  }

  private getAuthConfig(): AuthConfig {
    return {
      issuer: environment.iderntityHostUrl, // Url of the Identity Provider
      clientId: environment.clientId, // The SPA's id. The SPA is registerd with this id at the auth-server

      redirectUri: window.location.origin + '/index.html', // URL of the SPA to redirect the user to after login

      scope: 'openid profile email wasteproducts-api', // set the scope for the permissions the client should request

      showDebugInformation: !environment.production,
      sessionChecksEnabled: true,
    };
  }
}
