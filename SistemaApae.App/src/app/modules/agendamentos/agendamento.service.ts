import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Agendamento } from './agendamento';
import { Status } from '../core/enum/status.enum';

export interface AgendamentoFiltro {
  Nome?: string;
  Status?: Status;
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
        .get<{ data: Agendamento[] }>(`${this.baseUrl}Agendamento/filter`, { params: filtro as any })
        .pipe(map((r) => r.data)) || of([])
    );
  }

    salvar(agendamento: Agendamento): Observable<void> {  
      return this.http.post<void>(`${this.baseUrl}Agendamento`, agendamento);
    }
  
    editar(agendamento: Agendamento): Observable<void> {
      return this.http.put<void>(`${this.baseUrl}Agendamento`, agendamento);
    }
  
}
