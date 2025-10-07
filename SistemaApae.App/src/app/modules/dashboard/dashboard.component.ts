import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { PageInfoService } from '../core/services/page-info.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.less'],
})
export class DashboardComponent implements OnInit {
  constructor(private pageInfoService: PageInfoService, private router: Router) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Dashboard', 'Sistema de Gestão de Atendimentos');
  }

  // Métodos de navegação para os diferentes cadastros
  navigateToAssistidos(): void {
    this.router.navigate(['/home/assistidos/cadastro']);
  }

  navigateToUsuarios(): void {
    this.router.navigate(['/home/usuarios']);
  }

  navigateToAgendamentos(): void {
    this.router.navigate(['/home/agendamentos']);
  }

  navigateToAtendimentos(): void {
    this.router.navigate(['/home/atendimentos']);
  }
}
