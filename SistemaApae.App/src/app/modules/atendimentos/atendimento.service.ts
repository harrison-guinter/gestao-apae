import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Atendimento, StatusAtendimentoEnum } from './atendimento';
import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';
import { Usuario } from '../usuarios/usuario';

export interface AtendimentoFiltro {
  profissional?: Usuario;
  assistido?: Assistido;
  dataInicioAtendimento?: Date;
  dataFimAtendimento?: Date;
  presenca?: StatusAtendimentoEnum;
  status?: Status;
}

@Injectable({
  providedIn: 'root',
})
export class AtendimentoService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarAtendimentos(filtro: any = {}): Observable<Atendimento[]> {
    // Preparar parâmetros para a API
    const params: any = {};

    if (filtro.profissionalId) {
      params.idProfissional = filtro.profissionalId;
    }

    if (filtro.assistidoId) {
      params.idAssistido = filtro.assistidoId;
    }

    if (filtro.dataInicioAtendimento) {
      params.dataInicioAtendimento = filtro.dataInicioAtendimento.toISOString();
    }

    if (filtro.dataFimAtendimento) {
      params.dataFimAtendimento = filtro.dataFimAtendimento.toISOString();
    }

    if (filtro.presenca !== undefined) {
      params.presenca = filtro.presenca;
    }

    return this.http.get<{ data: any[] }>(`${this.baseUrl}Atendimento/filter`, { params }).pipe(
      map((response) => response.data.map((item) => new Atendimento(item))),
      map((atendimentos) => atendimentos)
    );
  }

  buscarPorId(id: string): Observable<Atendimento | null> {
    return this.http
      .get<{ data: any }>(`${this.baseUrl}Atendimento/${id}`)
      .pipe(map((response) => new Atendimento(response.data)));
  }

  buscarPorAssistido(assistido: Assistido): Observable<Atendimento[]> {
    return this.listarAtendimentos({ assistido });
  }

  salvar(atendimento: Atendimento): Observable<void> {
    console.log('atendimento service', atendimento);
    return this.http.post<void>(`${this.baseUrl}Atendimento`, atendimento);
  }

  editar(atendimento: Atendimento): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}Atendimento`, atendimento);
  }

  excluir(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}Atendimento/${id}`);
  }
}
