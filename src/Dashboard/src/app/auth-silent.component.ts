import { Component, OnInit } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-auth-silent',
  template: '<div class="wrapper wrapper-content animated"><h2>Waiting...</h2></div>'
})
export class AuthSilentComponent implements OnInit {

  constructor(private authService: AuthService) { }

  ngOnInit() {
    this.authService.endSignInSilentCallback();
  }

}
