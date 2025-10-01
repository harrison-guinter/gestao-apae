import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Assistido, StatusAssistidoEnum } from './assistido';
import { ApiResponse } from '../core/models/api-response.model';

export interface AssistidoFiltro {
  nome?: string;
  cpf?: string;
  status?: StatusAssistidoEnum;
  idMunicipio?: string;
}

@Injectable({
  providedIn: 'root',
})
export class AssistidoService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarAssistidos(filtros: AssistidoFiltro): Observable<Assistido[]> {
    const params = this.buildValidParams(filtros);

    return this.http
      .get<ApiResponse<Assistido[]>>(`${this.baseUrl}Assistido/filter`, { params })
      .pipe(
        map((response) => {
          const assistidos = response.data || [];
          // Ordenação alfabética pelo nome, ignorando maiúsculas e minúsculas
          return assistidos.sort((a, b) =>
            (a.nome || '').toLowerCase().localeCompare((b.nome || '').toLowerCase())
          );
        })
      );
  }

  obterAssistido(id: string): Observable<Assistido> {
    return this.http
      .get<ApiResponse<Assistido>>(`${this.baseUrl}Assistido/${id}`)
      .pipe(map((response) => response.data));
  }

  salvarAssistido(assistido: Assistido): Observable<Assistido> {
    const payload = this.buildAssistidoPayload(assistido, true);

    return this.http
      .post<ApiResponse<Assistido>>(`${this.baseUrl}Assistido`, payload)
      .pipe(map((response) => response.data));
  }

  editarAssistido(assistido: Assistido): Observable<Assistido> {
    const payload = this.buildAssistidoPayload(assistido, false);

    return this.http
      .put<ApiResponse<Assistido>>(`${this.baseUrl}Assistido`, payload)
      .pipe(map((response) => response.data));
  }

  private buildValidParams(filtros: AssistidoFiltro): any {
    const params: any = {};

    if (filtros.nome && filtros.nome.trim() !== '') {
      params.nome = filtros.nome.trim();
    }

    if (filtros.cpf && filtros.cpf.trim() !== '') {
      params.cpf = filtros.cpf.trim();
    }

    if (filtros.status !== undefined && filtros.status !== null) {
      params.status = filtros.status;
    }

    if (filtros.idMunicipio && filtros.idMunicipio.trim() !== '') {
      params.idMunicipio = filtros.idMunicipio.trim();
    }

    return params;
  }

  private buildAssistidoPayload(assistido: Assistido, isCreating: boolean): any {
    const payload: any = {
      nome: assistido.nome?.trim() || '',
      status: assistido.status || StatusAssistidoEnum.ATIVO,
    };

    // Campos opcionais - dados pessoais
    if (assistido.dataNascimento) payload.dataNascimento = assistido.dataNascimento;
    if (assistido.cpf) payload.cpf = assistido.cpf.trim();
    if (assistido.sexo) payload.sexo = assistido.sexo;
    if (assistido.naturalidade) payload.naturalidade = assistido.naturalidade.trim();
    if (assistido.nomeMae) payload.nomeMae = assistido.nomeMae.trim();
    if (assistido.nomePai) payload.nomePai = assistido.nomePai.trim();

    // Endereço
    if (assistido.endereco) payload.endereco = assistido.endereco.trim();
    if (assistido.bairro) payload.bairro = assistido.bairro.trim();
    if (assistido.cep) payload.cep = assistido.cep.trim();
    if (assistido.idMunicipio) payload.idMunicipio = assistido.idMunicipio;

    // Responsável
    if (assistido.nomeResponsavel) payload.nomeResponsavel = assistido.nomeResponsavel.trim();
    if (assistido.telefoneResponsavel)
      payload.telefoneResponsavel = assistido.telefoneResponsavel.trim();

    // Saúde
    if (assistido.tipoDeficiencia) payload.tipoDeficiencia = assistido.tipoDeficiencia;
    if (assistido.cid) payload.cid = assistido.cid.trim();
    if (assistido.medicamentosUso !== undefined)
      payload.medicamentosUso = assistido.medicamentosUso;
    if (assistido.medicamentosQuais) payload.medicamentosQuais = assistido.medicamentosQuais.trim();

    // Convênio
    if (assistido.idConvenio) payload.idConvenio = assistido.idConvenio;

    // Observações
    if (assistido.observacao) payload.observacao = assistido.observacao.trim();

    // Se está editando, inclui o ID
    if (!isCreating && assistido.id) {
      payload.id = assistido.id;
    }

    return payload;
  }
}
