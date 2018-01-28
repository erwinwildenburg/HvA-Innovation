import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './auth-guard.service';

import { AuthSilentComponent } from './auth-silent.component';
import { FilesComponent } from './files/files.component';
import { EditComponent } from './edit/edit.component';
import { AboutComponent } from './about/about.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';

// const appRoutes: Routes = [
//   { path: 'auth-silent', component: AuthSilentComponent },
//   { path: 'files', component: FilesComponent, canActivate: [AuthGuardService] },
//   { path: 'edit/:file', component: EditComponent, canActivate: [AuthGuardService] },
//   { path: 'about', component: AboutComponent, canActivate: [AuthGuardService] },
//   { path: '', pathMatch: 'full', redirectTo: '/files' },
//   { path: '**', component: PageNotFoundComponent, canActivate: [AuthGuardService] }
// ];

const appRoutes: Routes = [
  { path: 'auth-silent', component: AuthSilentComponent },
  { path: 'files', component: FilesComponent},
  { path: 'edit/:key/:title', component: EditComponent},
  { path: 'about', component: AboutComponent},
  { path: '', pathMatch: 'full', redirectTo: '/files' },
  { path: '**', component: PageNotFoundComponent}
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
