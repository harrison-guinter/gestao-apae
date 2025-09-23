import { Routes } from '@angular/router';

import { LoginComponent } from './modules/auth/login/login.component';
import { ResetPasswordComponent } from './modules/auth/reset-password/reset-password.component';
import { HomeComponent } from './modules/home/home.component';
import { UsuariosComponent } from './modules/usuarios/usuarios.component';
import { AssistidosComponent } from './modules/assistidos/assistidos.component';
import { DashboardComponent } from './modules/dashboard/dashboard.component';
import { AuthGuard } from './modules/auth/auth.guard';
import { NoAuthGuard } from './modules/auth/no-auth.guard';
import { CoordenadorGuard } from './modules/auth/coordenador.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  { path: 'reset-password', component: ResetPasswordComponent, canActivate: [NoAuthGuard] },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard, CoordenadorGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'usuarios', canActivate: [CoordenadorGuard], component: UsuariosComponent },
      { path: 'assistidos', canActivate: [CoordenadorGuard], component: AssistidosComponent },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
