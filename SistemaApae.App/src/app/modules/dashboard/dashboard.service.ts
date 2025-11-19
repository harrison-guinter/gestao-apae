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

  buscarDadosDashboard(): Observable<DashboardData> {
    return this.http.get<any>(`${this.baseUrl}Dashboard/stats`);
  }
}
