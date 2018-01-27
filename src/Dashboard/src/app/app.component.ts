import { Component, OnInit, AfterViewInit, HostListener, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { AuthService } from './auth.service';
import { environment } from '../environments/environment';
import { User } from 'oidc-client';
import { Subject } from 'rxjs/Subject';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  
  private ngUnsubscribe: Subject<void> = new Subject<void>();

  public firstName: string;
  public lastName: string;

  constructor(
    private authService: AuthService,
    private router: Router,
    private titleService: Title,
    private activatedRoute: ActivatedRoute
  ) {
    if (window.location.hash) {
      this.authService.endSignInMainWindow();
      this.router.navigate(['/']);
    }
  }

  ngOnInit()
  {
    this.authService.userLoadedEvent
      .takeUntil(this.ngUnsubscribe)
      .subscribe((user: User) => {
        if (this.authService.currentUser) {
          this.firstName = user.profile.given_name;
          this.lastName = user.profile.family_name;
        }
      });

    this.router.events
      .filter((event) => event instanceof NavigationEnd)
      .map(() => this.activatedRoute)
      .map((route) => {
        while (route.firstChild) {
          route = route.firstChild;
        }
        return route;
      })
      .filter((route) => route.outlet === 'primary')
      .mergeMap((route) => route.data)
      .takeUntil(this.ngUnsubscribe)
      .subscribe((event) => this.titleService.setTitle(environment.baseTitle + ' - ' + event['title']));
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  public logout(): void {
    this.authService.startSignOutMainWindow();
  }

  public setTitle(newTitle: string): void {
    this.titleService.setTitle(newTitle);
  }
}
