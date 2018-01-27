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

  file = {
    name: '<FILENAME>',
    // tslint:disable-next-line:max-line-length
    content: 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vestibulum convallis est nec molestie dignissim. Cras dui leo, aliquet vel eros et, faucibus lacinia justo. Duis eu nisl et tellus facilisis consequat. Nullam euismod, tellus aliquam faucibus ultrices, diam lectus pellentesque nunc, ut vulputate nibh ex in leo. Aenean fermentum, nibh a mollis maximus, elit nisi viverra libero, nec placerat nisi nisl id massa. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer quis risus lacus. Duis molestie enim vitae vehicula varius. Ut ut ex nisl. Praesent blandit luctus sem, sed porttitor tellus hendrerit vitae. Vestibulum ac dictum dui. Ut accumsan lorem nec ante mattis ultricies. Praesent fermentum convallis nunc, vitae sagittis tellus ornare tincidunt.',
    version: 1.0,
    language: 'english'
  };
}
