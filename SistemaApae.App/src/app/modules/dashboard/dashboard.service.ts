import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';

export interface DashboardData {
  totalAssistidos: number;
  atendimentosHoje: number;
  usuariosAtivos: number;
  agendamentosPendentes: number;
  atendimentosSemana: number;
  atendimentosMes: number;
  novosAssistidosMes: number;
  taxaPresenca: number;
}

@Injectable({
  providedIn: 'root',
})
export class DashboardService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  buscarDadosDashboard(year?: number, month?: number): Observable<DashboardData> {
    const params: any = {};
    if (year) params.year = year;
    if (month) params.month = month;

    return this.http.get<any>(`${this.baseUrl}Dashboard/stats`, { params });
  }
}
