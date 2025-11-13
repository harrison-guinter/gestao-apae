import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Agendamento } from './agendamento';
import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';

export interface AgendamentoFiltro {
  nome?: string;
  status?: Status;
  dataAgendamentoInicio?: string;
  dataAgendamentoFim?: string;
  assistidoId: string;
  profissionalId: string;
}

@Injectable({
  providedIn: 'root',
})
export class AgendamentoService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarAgendamentos(filtro: AgendamentoFiltro): Observable<Agendamento[]> {
    return (
      this.http
        .get<{ data: Agendamento[] }>(`${this.baseUrl}Agendamento/filter`, {
          params: filtro as any,
        })
        .pipe(map((r) => r.data)) || of([])
    );
  }

  salvar(agendamento: Agendamento): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}Agendamento`, agendamento);
  }

  editar(agendamento: Agendamento): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}Agendamento`, agendamento);
  }

  listarAgendamentosPorProfissional(idProfissional: string, dataAgendamento: string): Observable<Agendamento[]> {
    return this.http.get<{ data: Agendamento[] }>(`${this.baseUrl}Agendamento/profissional/${idProfissional}`, { params: {data: dataAgendamento}}).pipe(map((r) => r.data)) || of([]);
  }
}
