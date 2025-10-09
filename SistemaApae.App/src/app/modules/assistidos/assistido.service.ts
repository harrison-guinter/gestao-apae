import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { Assistido, StatusAssistidoEnum, PlanoSaudeEnum } from './assistido';
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
    // Campos obrigatórios
    const payload: any = {
      nome: assistido.nome?.trim() || '',
      status: assistido.status || StatusAssistidoEnum.ATIVO,
      medicamentosUso: assistido.medicamentosUso ?? false,
    };

    // Campos que devem ser trimmed (strings)
    const stringFields = [
      'cpf',
      'naturalidade',
      'nomeMae',
      'nomePai',
      'endereco',
      'bairro',
      'cep',
      'nomeResponsavel',
      'telefoneResponsavel',
      'responsavelBusca',
      'cid',
      'medicamentosQuais',
      'nomeEscola',
      'anoEscola',
      'composicaoFamiliar',
      'apegoFamiliar',
      'caracteristicasMarcantes',
      'medicoResponsavel',
      'examesRealizados',
      'doencasFisicas',
      'qualidadeSono',
      'cirurgiasRealizadas',
      'doencasNeurologicas',
      'historicoFamiliarDoencas',
      'descricaoGestacao',
      'usoMedicacaoMae',
      'descricaoDemanda',
      'observacao',
    ];

    // Campos booleanos
    const booleanFields = [
      'acompanhamentoEspecializado',
      'bpc',
      'bolsaFamilia',
      'passeLivreEstadual',
      'passeLivreMunicipal',
      'paisCasados',
      'paternidadeRegistrada',
      'consentimentoImagem',
      'boaSocializacao',
      'boaAdaptacao',
      'comportamentoAgressivo',
      'controleEsfincteres',
      'atrasoAlimentacao',
      'atrasoHigiene',
      'atrasoVestuario',
      'atrasoLocomocao',
      'atrasoComunicacao',
      'internacaoPosNascimento',
    ];

    // Campos de valor direto (IDs, enums, números, datas)
    const directFields = [
      'dataNascimento',
      'sexo',
      'idMunicipio',
      'tipoDeficiencia',
      'planoSaude',
      'turnoEscola',
      'gestacaoSemanas',
      'idConvenio',
    ];

    // Processar campos de string (com trim)
    stringFields.forEach((field) => {
      const value = assistido[field as keyof Assistido];
      if (value && typeof value === 'string' && value.trim()) {
        payload[field] = value.trim();
      }
    });

    // Processar campos booleanos
    booleanFields.forEach((field) => {
      const value = assistido[field as keyof Assistido];
      if (value !== undefined && value !== null) {
        payload[field] = value;
      }
    });

    // Processar campos de valor direto
    directFields.forEach((field) => {
      const value = assistido[field as keyof Assistido];
      if (value !== undefined && value !== null) {
        payload[field] = value;
      }
    });

    // Se está editando, inclui o ID
    if (!isCreating && assistido.id) {
      payload.id = assistido.id;
    }

    return payload;
  }
}
