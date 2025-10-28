import { inject, Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { Observable } from 'rxjs';
import { DashboardData, DashboardService } from './dashboard.service';

@Injectable({ providedIn: 'root' })
export class DashboardResolver implements Resolve<DashboardData> {
  private service = inject(DashboardService);

  resolve(): Observable<DashboardData> | Promise<DashboardData> | DashboardData {
    return this.service.buscarDadosDashboard();
  }
}
