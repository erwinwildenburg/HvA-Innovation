import { Injectable, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { UserManager, Log, MetadataService, User } from 'oidc-client';
import { environment } from '../environments/environment';

const settings: any = {
  authority: environment.authority,
  client_id: environment.client_id,
  redirect_uri: window.location.origin,
  post_logout_redirect_uri: window.location.origin,
  response_type: environment.response_type,
  scope: environment.scope,
  silent_redirect_uri: window.location.origin + '/auth-silent',
  automaticSilentRenew: true,
  accessTokenExpiringNotificationTime: 60,
  filterProtocolClaims: false,
  loadUserInfo: true,
  revokeAccessTokenOnSignout: true
};

@Injectable()
export class AuthService {

  private mgr: UserManager = new UserManager(settings);
  public userLoadedEvent: EventEmitter<User> = new EventEmitter<User>();
  public currentUser: User;
  public loggedIn = false;
  private authHeaders: Headers;

  constructor(private Http: HttpClient) {
    this.mgr.getUser()
      .then((user) => {
        if (user) {
          this.loggedIn = true;
          this.currentUser = user;
          this.userLoadedEvent.emit(user);
        } else {
          this.loggedIn = false;
        }
      }).catch((err) => {
        this.loggedIn = false;
      });

    this.mgr.events.addUserLoaded(user => {
      this.currentUser = user;
      this.userLoadedEvent.emit(user);
      if (!environment.production) { console.log('authService addUserLoaded', user); }
    });

    this.mgr.events.addUserUnloaded((e) => {
      this.loggedIn = false;
      if (!environment.production) { console.log('authService addUserUnloaded'); }
    });
  }

  public clearState(): void {
    this.mgr.clearStaleState().then(function () {
      if (!environment.production) { console.log('clearStaleState success'); }
    }).catch(function (err) {
      console.error('clearStaleState error: ', err.message);
    });
  }

  public getUser(): void {
    this.mgr.getUser().then((user) => {
      this.currentUser = user;
      this.userLoadedEvent.emit(user);
      if (!environment.production) { console.log('getUser success', user); }
    }).catch(function (err) {
      console.error('getUser error: ', err.message);
    });
  }

  public isLoggedInObs(): Observable<boolean> {
    return Observable.fromPromise(this.mgr.getUser()).map<User, boolean>((user) => {
      if (user) {
        return true;
      } else {
        return false;
      }
    });
  }

  public removeUser(): void {
    this.mgr.removeUser().then(() => {
      this.userLoadedEvent.emit(null);
      if (!environment.production) { console.log('removeUser success'); }
    }).catch(function (err) {
      console.error('removeUser error: ', err.message);
    });
  }

  public startSignInMainWindow(): void {
    this.mgr.signinRedirect().then(function () {
      if (!environment.production) { console.log('signInRedirect success'); }
    }).catch(function (err) {
      console.error('signInRedirect error: ', err.message);
    });
  }

  public endSignInMainWindow(): void {
    this.mgr.signinRedirectCallback().then(function (user) {
      if (!environment.production) { console.log('signed in', user); }
    }).catch(function (err) {
      console.error('signin error: ', err.message);
    });
  }

  public startSignOutMainWindow(): void {
    this.mgr.signoutRedirect().then(function (resp) {
      if (!environment.production) { console.log('signed out', resp); }
    }).catch(function (err) {
      console.error('signout error: ', err.message);
    });
  }

  public endSignOutMainWindow(): void {
    this.mgr.signoutRedirectCallback().then(function (resp) {
      if (!environment.production) { console.log('signed out', resp); }
    }).catch(function (err) {
      console.error('signout error: ', err.message);
    });
  }

  public endSignInSilentCallback(): void {
    this.mgr.signinSilentCallback().then(function (resp) {
      if (!environment.production) { console.log('renewed', resp); }
    }).catch(function(err) {
      console.error('renew error: ', err.message);
    });
  }

  private getAppUrl(): string {
    let appUrl = window.location.protocol + '//' + window.location.hostname;
    if (window.location.port !== '') {
        appUrl = appUrl + ':' + window.location.port;
    }

    return appUrl;
  }
}
