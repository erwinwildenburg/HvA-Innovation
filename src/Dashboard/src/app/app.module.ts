import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule, Routes } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { environment } from '../environments/environment';

import { AppComponent } from './app.component';

import { AppRoutingModule } from './app-routing.module';

import { AuthService } from './auth.service';
import { AuthGuardService } from './auth-guard.service';
import { AuthHttpService } from './auth-http.service';
import { AuthSilentComponent } from './auth-silent.component';

import { FroalaEditorModule, FroalaViewModule } from 'angular-froala-wysiwyg';
import { FilesComponent } from './files/files.component';
import { FilesService } from './files/files.service';
import { EditComponent } from './edit/edit.component';
import { AboutComponent } from './about/about.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/fromPromise';
import 'rxjs/add/operator/takeUntil';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/filter';

@NgModule({
  declarations: [
    AppComponent,
    AuthSilentComponent,
    FilesComponent,
    EditComponent,
    AboutComponent,
    PageNotFoundComponent
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
    AuthService,
    FilesService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
