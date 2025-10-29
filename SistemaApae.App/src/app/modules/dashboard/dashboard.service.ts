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
    const params: any = {};

    return this.http.get<{ data: any }>(`${this.baseUrl}Dashboard/stats`).pipe(
      map((response) => ({
        totalAssistidos: response.data.TotalAssistidos,
        atendimentosHoje: response.data.AtendimentosHoje,
        usuariosAtivos: response.data.UsuariosAtivos,
        agendamentosPendentes: response.data.AgendamentosPendentes,
        atendimentosSemana: response.data.AtendimentosSemana,
        atendimentosMes: response.data.AtendimentosMes,
        novosAssistidosMes: response.data.NovosAssistidosMes,
        taxaPresenca: response.data.TaxaPresenca,
      }))
    );
  }
}
