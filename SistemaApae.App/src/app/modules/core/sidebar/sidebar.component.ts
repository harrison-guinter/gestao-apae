import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, NavigationEnd, RouterModule } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatExpansionModule } from '@angular/material/expansion';
import { filter } from 'rxjs/operators';
import { PageInfoService } from '../services/page-info.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
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
export class SidebarComponent implements OnInit {
  constructor(private router: Router, private pageInfoService: PageInfoService) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfoByRoute(this.router.url);

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: NavigationEnd) => {
        this.pageInfoService.updatePageInfoByRoute(event.url);
      });
  }

  isProfissional(): boolean {
    return JSON.parse(localStorage.getItem('usuario')!).perfil === 'Profissional';
  }

  menuItems = [
    { icon: 'dashboard', label: 'Dashboard', route: '/home/dashboard' },
    { icon: 'groups', label: 'Assistidos', route: '/home/assistidos' },
    { icon: 'handshake', label: 'Convênios', route: '/home/convenios' },
    { icon: 'event', label: 'Agendamentos', route: '/home/agendamentos' },
    {
      icon: 'medical_services',
      label: 'Atendimentos',
      route: this.isProfissional()
        ? '/home/atendimentos-pendentes'
        : '/home/atendimentos-realizados',
    },
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
