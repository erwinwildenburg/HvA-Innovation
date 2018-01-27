import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuardService } from './auth-guard.service';

import { HomeComponent } from './home.component';
import { AuthSilentComponent } from './auth-silent.component';

const appRoutes: Routes = [
  { path: 'auth-silent', component: AuthSilentComponent },
  { path: '**', component: HomeComponent, canActivate: [AuthGuardService] }
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
