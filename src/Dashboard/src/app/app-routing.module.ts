import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './auth-guard.service';

import { HomeComponent } from './home.component';
import { AuthSilentComponent } from './auth-silent.component';
import { FilesComponent } from './files/files.component';
import { EditComponent } from './edit/edit.component';
import { AboutComponent } from './about/about.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

const appRoutes: Routes = [
  { path: 'auth-silent', component: AuthSilentComponent },
  { path: 'files', component: FilesComponent, canActivate: [AuthGuardService] },
  { path: 'edit/:file', component: EditComponent, canActivate: [AuthGuardService] },
  { path: 'about', component: AboutComponent, canActivate: [AuthGuardService] },
  { path: '', redirectTo: '/files', pathMatch: 'full', canActivate: [AuthGuardService] },
  { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuardService] }
];

@NgModule({
  imports: [
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: false } // <-- debugging purposes only
    )
  ],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {}
