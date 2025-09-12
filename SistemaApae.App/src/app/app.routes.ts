import { Routes } from '@angular/router';

import { LoginComponent } from './modules/auth/login/login.component';
import { ResetPasswordComponent } from './modules/auth/reset-password/reset-password.component';
import { HomeComponent } from './modules/home/home.component';
import { UsuariosComponent } from './modules/usuarios/usuarios.component';
import { AssistidosComponent } from './modules/assistidos/assistidos.component';
import { DashboardComponent } from './modules/dashboard/dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: 'home',
    component: HomeComponent,
    //canActivate: [AuthGuard],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'usuarios', component: UsuariosComponent },
      { path: 'assistidos', component: AssistidosComponent },
      // Adicione outras rotas aqui conforme necessário
    ],
  },
  { path: '**', redirectTo: 'login' },
];
