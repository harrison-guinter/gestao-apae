import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; // Para routerLink
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';

@Component({
  selector: 'app-sidebar',
  standalone: true, // se for Angular 15+
  imports: [
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatExpansionModule,
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less'],
})
export class SidebarComponent {
  menuItems = [
    { icon: 'dashboard', label: 'Dashboard', route: '/home/dashboard' },
    { icon: 'groups', label: 'Assistidos', route: '/home/assistidos' },
    { icon: 'handshake', label: 'Convênios', route: '/home/convenios' },
    { icon: 'event', label: 'Agendamentos', route: '/home/agendamentos' },
    { icon: 'medical_services', label: 'Atendimentos', route: '/home/atendimentos' },
    {
      icon: 'bar_chart',
      label: 'Relatórios',
      children: [
        {
          icon: 'assignment',
          label: 'Atendimentos',
          route: '/home/relatorios/atendimentos',
        },
        { icon: 'event_busy', label: 'Faltas', route: '/home/relatorios/faltas' },
        { icon: 'person', label: 'Individual', route: '/home/relatorios/individual' },
        {
          icon: 'check_circle',
          label: 'Presenças',
          route: '/home/relatorios/presencas',
        },
      ],
    },
    { icon: 'person_outline', label: 'Usuários', route: '/home/usuarios' },
  ];
}
