import { Routes } from '@angular/router';

import { LoginComponent } from './modules/auth/login/login.component';
import { ResetPasswordComponent } from './modules/auth/reset-password/reset-password.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' }, 
  { path: 'login', component: LoginComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  // { path: 'home', component: HomeComponent, canActivate: [AuthGuard] }, 
  { path: '**', redirectTo: 'login' } 
];