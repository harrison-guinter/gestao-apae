import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { map, Observable, of } from 'rxjs';
import { Atendimento, StatusAtendimentoEnum } from './atendimento';
import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';
import { Agendamento } from '../agendamentos/agendamento';

export interface AtendimentoFiltro {
  idAgendamento?: string;
  idAssistido?: string;
  dataInicio?: Date;
  dataFim?: Date;
  presenca?: StatusAtendimentoEnum;
  status?: Status;
}

@Injectable({
  providedIn: 'root',
})
export class AtendimentoService {
  private baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  listarAtendimentos(filtro: AtendimentoFiltro = {}): Observable<Atendimento[]> {
    // Por enquanto retornando dados mockados
    return this.getMockAtendimentos(filtro);

    // Implementação real da API comentada:
    /*
    return this.http
      .get<{ data: Atendimento[] }>(`${this.baseUrl}Atendimento/filter`, { params: filtro as any })
      .pipe(map((r) => r.data.map(item => new Atendimento(item)))) || of([]);
    */
  }

  buscarPorId(id: string): Observable<Atendimento | null> {
    // Por enquanto retornando dados mockados
    return this.getMockAtendimentos().pipe(
      map((atendimentos) => atendimentos.find((a) => a.id === id) || null)
    );

    // Implementação real da API comentada:
    /*
    return this.http
      .get<{ data: Atendimento }>(`${this.baseUrl}Atendimento/${id}`)
      .pipe(map((r) => new Atendimento(r.data)));
    */
  }

  buscarPorAgendamento(idAgendamento: string): Observable<Atendimento[]> {
    // Por enquanto retornando dados mockados
    return this.getMockAtendimentos({ idAgendamento });

    // Implementação real da API comentada:
    /*
    return this.http
      .get<{ data: Atendimento[] }>(`${this.baseUrl}Atendimento/agendamento/${idAgendamento}`)
      .pipe(map((r) => r.data.map(item => new Atendimento(item))));
    */
  }

  buscarPorAssistido(idAssistido: string): Observable<Atendimento[]> {
    // Por enquanto retornando dados mockados
    return this.getMockAtendimentos({ idAssistido });

    // Implementação real da API comentada:
    /*
    return this.http
      .get<{ data: Atendimento[] }>(`${this.baseUrl}Atendimento/assistido/${idAssistido}`)
      .pipe(map((r) => r.data.map(item => new Atendimento(item))));
    */
  }

  salvar(atendimento: Atendimento): Observable<Atendimento> {
    // Por enquanto simulando criação
    const novoAtendimento = new Atendimento({
      ...atendimento,
      id: this.generateId(),
      createdAt: new Date(),
      status: Status.Ativo,
    });
    return of(novoAtendimento);

    // Implementação real da API comentada:
    /*
    return this.http
      .post<{ data: Atendimento }>(`${this.baseUrl}Atendimento`, atendimento)
      .pipe(map((r) => new Atendimento(r.data)));
    */
  }

  editar(atendimento: Atendimento): Observable<Atendimento> {
    // Por enquanto simulando edição
    const atendimentoAtualizado = new Atendimento({
      ...atendimento,
      updatedAt: new Date(),
    });
    return of(atendimentoAtualizado);

    // Implementação real da API comentada:
    /*
    return this.http
      .put<{ data: Atendimento }>(`${this.baseUrl}Atendimento/${atendimento.id}`, atendimento)
      .pipe(map((r) => new Atendimento(r.data)));
    */
  }

  excluir(id: string): Observable<void> {
    // Por enquanto simulando exclusão
    return of();

    // Implementação real da API comentada:
    /*
    return this.http.delete<void>(`${this.baseUrl}Atendimento/${id}`);
    */
  }

  // Métodos auxiliares para dados mockados
  private getMockAtendimentos(filtro: AtendimentoFiltro = {}): Observable<Atendimento[]> {
    const mockAtendimentos = [
      new Atendimento({
        id: 'AT001',
        idAgendamento: 'AG001',
        idAssistido: 'A2',
        dataAtendimento: new Date('2025-10-07T09:00:00'),
        presenca: StatusAtendimentoEnum.PRESENTE,
        avaliacao:
          'Ótima evolução nos exercícios de coordenação motora. Paciente demonstrou melhora significativa.',
        observacao: 'Paciente chegou no horário e participou ativamente da sessão.',
        status: Status.Ativo,
        createdAt: new Date('2025-10-07T09:00:00'),
        assistido: {
          id: 'A2',
          nome: 'Carlos Silva',
          dataNascimento: '2010-11-22',
          endereco: 'Av. Central, 456',
          status: Status.Ativo,
          sexo: 'MASCULINO',
          tipoDeficiencia: 'FISICA',
          medicamentosUso: false,
          nomeResponsavel: 'Ana Silva',
          telefoneResponsavel: '(49) 98888-2222',
          descricaoDemanda: 'Fisioterapia motora semanal',
          acompanhamentoEspecializado: true,
          nomeEscola: 'Colégio Estadual Horizonte',
          turnoEscola: 'VESPERTINO',
        },
        agendamento: {
          id: 'AG001',
          nome: 'Sessão de Fisioterapia',
          observacao: 'Primeira sessão de avaliação motora',
          status: Status.Ativo,
          data: new Date('2025-10-07'),
          hora: '09:00',
        },
      }),
      new Atendimento({
        id: 'AT002',
        idAgendamento: 'AG002',
        idAssistido: 'A1',
        dataAtendimento: new Date('2025-10-08T14:00:00'),
        presenca: StatusAtendimentoEnum.PRESENTE,
        avaliacao:
          'Sessão produtiva. Paciente demonstrou boa receptividade às atividades propostas.',
        observacao: 'Responsável relatou melhora no comportamento em casa.',
        status: Status.Ativo,
        createdAt: new Date('2025-10-08T14:00:00'),
        assistido: {
          id: 'A1',
          nome: 'Maria Souza',
          dataNascimento: '2012-05-10',
          endereco: 'Rua das Flores, 123',
          status: Status.Ativo,
          sexo: 'FEMININO',
          tipoDeficiencia: 'INTELECTUAL',
          medicamentosUso: true,
          medicamentosQuais: 'Ritalina',
          nomeResponsavel: 'João Souza',
          telefoneResponsavel: '(49) 99999-1111',
          descricaoDemanda: 'Dificuldades de aprendizagem',
          acompanhamentoEspecializado: true,
          nomeEscola: 'Escola Municipal Esperança',
          turnoEscola: 'MATUTINO',
        },
        agendamento: {
          id: 'AG002',
          nome: 'Atendimento Psicológico',
          observacao: 'Sessão de acompanhamento emocional',
          status: Status.Ativo,
          data: new Date('2025-10-08'),
          hora: '14:00',
        },
      }),
      new Atendimento({
        id: 'AT003',
        idAgendamento: 'AG001',
        idAssistido: 'A2',
        dataAtendimento: new Date('2025-10-05T09:00:00'),
        presenca: StatusAtendimentoEnum.FALTA,
        avaliacao: '',
        observacao: 'Paciente não compareceu. Responsável não atendeu ligação.',
        status: Status.Ativo,
        createdAt: new Date('2025-10-05T09:00:00'),
        assistido: {
          id: 'A2',
          nome: 'Carlos Silva',
          dataNascimento: '2010-11-22',
          endereco: 'Av. Central, 456',
          status: Status.Ativo,
          sexo: 'MASCULINO',
          tipoDeficiencia: 'FISICA',
          medicamentosUso: false,
          nomeResponsavel: 'Ana Silva',
          telefoneResponsavel: '(49) 98888-2222',
          descricaoDemanda: 'Fisioterapia motora semanal',
          acompanhamentoEspecializado: true,
          nomeEscola: 'Colégio Estadual Horizonte',
          turnoEscola: 'VESPERTINO',
        },
        agendamento: {
          id: 'AG001',
          nome: 'Sessão de Fisioterapia',
          observacao: 'Primeira sessão de avaliação motora',
          status: Status.Ativo,
          data: new Date('2025-10-05'),
          hora: '09:00',
        },
      }),
      new Atendimento({
        id: 'AT004',
        idAgendamento: 'AG002',
        idAssistido: 'A1',
        dataAtendimento: new Date('2025-10-01T14:00:00'),
        presenca: StatusAtendimentoEnum.FALTA_JUSTIFICADA,
        avaliacao: '',
        observacao: 'Paciente com febre. Responsável justificou a ausência por telefone.',
        status: Status.Ativo,
        createdAt: new Date('2025-10-01T14:00:00'),
        assistido: {
          id: 'A1',
          nome: 'Maria Souza',
          dataNascimento: '2012-05-10',
          endereco: 'Rua das Flores, 123',
          status: Status.Ativo,
          sexo: 'FEMININO',
          tipoDeficiencia: 'INTELECTUAL',
          medicamentosUso: true,
          medicamentosQuais: 'Ritalina',
          nomeResponsavel: 'João Souza',
          telefoneResponsavel: '(49) 99999-1111',
          descricaoDemanda: 'Dificuldades de aprendizagem',
          acompanhamentoEspecializado: true,
          nomeEscola: 'Escola Municipal Esperança',
          turnoEscola: 'MATUTINO',
        },
        agendamento: {
          id: 'AG002',
          nome: 'Atendimento Psicológico',
          observacao: 'Sessão de acompanhamento emocional',
          status: Status.Ativo,
          data: new Date('2025-10-01'),
          hora: '14:00',
        },
      }),
    ];

    let filteredAtendimentos = mockAtendimentos;

    // Aplicar filtros
    if (filtro.idAgendamento) {
      filteredAtendimentos = filteredAtendimentos.filter(
        (a) => a.idAgendamento === filtro.idAgendamento
      );
    }

    if (filtro.idAssistido) {
      filteredAtendimentos = filteredAtendimentos.filter(
        (a) => a.idAssistido === filtro.idAssistido
      );
    }

    if (filtro.dataInicio) {
      filteredAtendimentos = filteredAtendimentos.filter(
        (a) => a.dataAtendimento && a.dataAtendimento >= filtro.dataInicio!
      );
    }

    if (filtro.dataFim) {
      filteredAtendimentos = filteredAtendimentos.filter(
        (a) => a.dataAtendimento && a.dataAtendimento <= filtro.dataFim!
      );
    }

    if (filtro.presenca !== undefined) {
      filteredAtendimentos = filteredAtendimentos.filter((a) => a.presenca === filtro.presenca);
    }

    if (filtro.status !== undefined) {
      filteredAtendimentos = filteredAtendimentos.filter((a) => a.status === filtro.status);
    }

    return of(filteredAtendimentos);
  }

  private generateId(): string {
    return 'AT' + Math.random().toString(36).substr(2, 9).toUpperCase();
  }
}
