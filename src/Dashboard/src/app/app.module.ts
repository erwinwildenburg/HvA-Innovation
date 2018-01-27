import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { environment } from '../environments/environment';

import { AppComponent } from './app.component';
import { HomeComponent } from './home.component';

import { AppRoutingModule } from './app-routing.module';

import { AuthService } from './auth.service';
import { AuthGuardService } from './auth-guard.service';
import { AuthHttpService } from './auth-http.service';
import { AuthSilentComponent } from './auth-silent.component';

import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/filter';

// import { translate as Translate } from 'google-translate-api';
// import { languages as Languages } from 'google-translate-api';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthSilentComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    FroalaEditorModule.forRoot(),
    FroalaViewModule.forRoot()
  ],
  providers: [
    AuthHttpService,
    AuthGuardService,
    AuthService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
