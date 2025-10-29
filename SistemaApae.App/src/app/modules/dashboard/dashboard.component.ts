import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { PageInfoService } from '../core/services/page-info.service';
import { DashboardData } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],
})
export class DashboardComponent implements OnInit {
  dashboardData: DashboardData | null = null;

  constructor(
    private pageInfoService: PageInfoService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Dashboard', 'Sistema de Gestão de Atendimentos');
    this.activatedRoute.data.subscribe(({ data }) => {
      this.dashboardData = data;
      console.log('Dashboard data loaded:', this.dashboardData);
    });
  }

  // Métodos de navegação para os diferentes cadastros
  navigateToAssistidos(): void {
    this.router.navigate(['/home/assistidos/cadastro']);
  }

  navigateToUsuarios(): void {
    this.router.navigate(['/home/usuarios'], { queryParams: { isNew: 'true' } });
  }

  navigateToAgendamentos(): void {
    this.router.navigate(['/home/agendamentos']);
  }

  navigateToAtendimentos(): void {
    this.router.navigate(['/home/atendimentos']);
  }
}
