import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageInfoService } from '../core/services/page-info.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="dashboard-container">
      <h2>Dashboard</h2>
      <p>Bem-vindo ao sistema de gestão APAE!</p>
      <div class="stats-grid">
        <div class="stat-card">
          <h3>Total de Assistidos</h3>
          <span class="stat-number">150</span>
        </div>
        <div class="stat-card">
          <h3>Atendimentos Hoje</h3>
          <span class="stat-number">25</span>
        </div>
        <div class="stat-card">
          <h3>Usuários Ativos</h3>
          <span class="stat-number">12</span>
        </div>
      </div>
    </div>
  `,
  styles: [
    `
      .dashboard-container {
        padding: 20px;
      }
      .stats-grid {
        display: grid;
        grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
        gap: 20px;
        margin-top: 20px;
      }
      .stat-card {
        background: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        text-align: center;
      }
      .stat-number {
        font-size: 2rem;
        font-weight: bold;
        color: #3f51b5;
      }
    `,
  ],
})
export class DashboardComponent implements OnInit {
  constructor(private pageInfoService: PageInfoService) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Dashboard', 'Sistema de Gestão de Atendimentos');
  }
}
