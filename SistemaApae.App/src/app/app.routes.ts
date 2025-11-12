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
import { ConveniosComponent } from './modules/convenios/convenios.component';
import { AgendamentosComponent } from './modules/agendamentos/agendamentos.component';
import { ProfissionalGuard } from './modules/auth/profissional.guard';
import { CadastroAssistidoComponent } from './modules/assistidos/cadastro-assistido/cadastro-assistido.component';
import { AtendimentosRealizadosComponent } from './modules/atendimentos/atendimentos-realizados/atendimentos-realizados.component';
import { DashboardResolver } from './modules/dashboard/dashboard.resolver';
import { AtendimentosPendentesComponent } from './modules/atendimentos/atendimentos-pendentes/atendimentos-pendentes.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [NoAuthGuard] },
  { path: 'reset-password', component: ResetPasswordComponent, canActivate: [NoAuthGuard] },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        component: DashboardComponent,
        // resolve: { DashboardResolver }
      },
      { path: 'usuarios', canActivate: [CoordenadorGuard], component: UsuariosComponent },
      { path: 'assistidos', canActivate: [CoordenadorGuard], component: AssistidosComponent },
      {
        path: 'assistidos/cadastro',
        canActivate: [CoordenadorGuard],
        component: CadastroAssistidoComponent,
      },
      {
        path: 'assistidos/cadastro/:id',
        canActivate: [CoordenadorGuard],
        component: CadastroAssistidoComponent,
      },
      { path: 'convenios', canActivate: [CoordenadorGuard], component: ConveniosComponent },
      { path: 'agendamentos', canActivate: [ProfissionalGuard], component: AgendamentosComponent },
      {
        path: 'atendimentos-pendentes',
        canActivate: [ProfissionalGuard],
        component: AtendimentosPendentesComponent,
      },
      {
        path: 'atendimentos-realizados',
        canActivate: [CoordenadorGuard],
        component: AtendimentosRealizadosComponent,
      },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
