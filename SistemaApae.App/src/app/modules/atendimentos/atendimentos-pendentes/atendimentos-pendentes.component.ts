import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TableComponent, TableColumn, TableAction } from '../../core/table/table.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { PageInfoService } from '../../core/services/page-info.service';
import { FiltersContainerComponent } from '../../core/filters-container/filters-container.component';
import { ModalService } from '../../core/services/modal.service';
import { Atendimento, StatusAtendimentoEnum } from '../atendimento';
import { ModalAtendimentosComponent } from '../modal-atendimentos/modal-atendimentos.component';
import { AtendimentoService, AtendimentoFiltro } from '../atendimento.service';
import { Status } from '../../core/enum/status.enum';
import { Assistido } from '../../assistidos/assistido';
import { Agendamento } from '../../agendamentos/agendamento';
import { AssistidoService } from '../../assistidos/assistido.service';
import { AgendamentoService } from '../../agendamentos/agendamento.service';
import { map, Observable } from 'rxjs';
import { Usuario } from '../../usuarios/usuario';
import { DatepickerComponent as DateComponent } from '../../core/date/datepicker/datepicker.component';

// Interface para representar a linha da tabela (combinação assistido/atendimento)
interface AtendimentoPendente {
  agendamento: any;
  assistido: any;
  atendimento: any;
  dataAtendimento?: Date;
  presenca?: StatusAtendimentoEnum;
  avaliacao?: string;
  status?: Status;
}

@Component({
  selector: 'app-atendimentos',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    TableComponent,
    SelectComponent,
    FiltersContainerComponent,
    DateComponent,
  ],
  templateUrl: './atendimentos-pendentes.component.html',
  styleUrls: ['./atendimentos-pendentes.component.less'],
})
export class AtendimentosPendentesComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private atendimentoService = inject(AtendimentoService);
  private assistidoService = inject(AssistidoService);
  private agendamentoService = inject(AgendamentoService);

  protected atendimentosPendentes: AtendimentoPendente[] = [];
  private atendimentosPendentesOriginais: AtendimentoPendente[] = [];

  assistidoOptions: Observable<SelectOption[]> = this.assistidoService.listarAssistidos({}).pipe(
    map((assistidos) =>
      assistidos.map((assistido) => ({
        value: assistido.id,
        label: assistido.nome,
      }))
    )
  );

  agendamentoOptions: Observable<SelectOption[]> = this.agendamentoService
    .listarAgendamentos({} as any)
    .pipe(
      map((agendamentos) =>
        agendamentos.map((agendamento) => ({
          value: agendamento.id,
          label: `${agendamento.nome} - ${agendamento.dataAgendamento.toLocaleDateString()} ${
            agendamento.horarioAgendamento
          }`,
        }))
      )
    );

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Atendimentos', 'Gerenciar atendimentos do sistema');

    this.initFiltrosForm();
    this.pesquisarAtendimentos();
  }

  pesquisarAtendimentos() {
    // Obter o ID do profissional logado
    const usuarioLogado = Usuario.getCurrentUser();

    if (!usuarioLogado || !usuarioLogado.id) {
      console.error('Usuário não está logado ou não possui ID');
      return;
    }

    this.agendamentoService.listarAgendamentosPorProfissional(usuarioLogado.id).subscribe({
      next: (agendamentos) => {
        this.processarAgendamentos(agendamentos);
      },
      error: (error) => {
        console.error('Erro ao pesquisar agendamentos do profissional:', error);
      },
    });
  }

  private processarAgendamentos(agendamentos: any[]) {
    this.atendimentosPendentesOriginais = [];

    if (!agendamentos || agendamentos.length === 0) {
      this.atendimentosPendentes = [];
      return;
    }

    agendamentos.forEach((agendamento) => {
      // Garantir que assistidos existe e é um array
      const assistidos = agendamento.assistidos || [];
      const atendimentos = agendamento.atendimentos || [];

      if (assistidos.length > 0) {
        if (atendimentos.length > 0) {
          // Caso 1: Há assistidos e atendimentos - criar combinações
          assistidos.forEach((assistido: any) => {
            atendimentos.forEach((atendimento: any) => {
              // Verificar se este atendimento pertence a este assistido
              if (!atendimento.idAssistido || atendimento.idAssistido === assistido.id) {
                const atendimentoPendente: AtendimentoPendente = {
                  agendamento: agendamento,
                  assistido: assistido,
                  atendimento: atendimento,
                  dataAtendimento: this.extrairDataAtendimento(atendimento, agendamento),
                  presenca: atendimento.presenca,
                  avaliacao: atendimento.avaliacao,
                  status: atendimento.status !== undefined ? atendimento.status : Status.Ativo,
                };
                this.atendimentosPendentesOriginais.push(atendimentoPendente);
              }
            });
          });
        } else {
          // Caso 2: Há assistidos mas não há atendimentos - criar linhas pendentes
          assistidos.forEach((assistido: any) => {
            const atendimentoPendente: AtendimentoPendente = {
              agendamento: agendamento,
              assistido: assistido,
              atendimento: null,
              dataAtendimento: this.extrairDataAtendimento(null, agendamento),
              presenca: undefined,
              avaliacao: undefined,
              status: Status.Ativo,
            };
            this.atendimentosPendentesOriginais.push(atendimentoPendente);
          });
        }
      } else if (atendimentos.length > 0) {
        // Caso 3: Há atendimentos mas não há assistidos listados explicitamente
        atendimentos.forEach((atendimento: any) => {
          const atendimentoPendente: AtendimentoPendente = {
            agendamento: agendamento,
            assistido: null, // Será necessário buscar o assistido pelo ID se disponível
            atendimento: atendimento,
            dataAtendimento: this.extrairDataAtendimento(atendimento, agendamento),
            presenca: atendimento.presenca,
            avaliacao: atendimento.avaliacao,
            status: atendimento.status !== undefined ? atendimento.status : Status.Ativo,
          };
          this.atendimentosPendentesOriginais.push(atendimentoPendente);
        });
      }
    });

    // Aplicar filtros localmente se necessário
    this.aplicarFiltros();
  }

  private extrairDataAtendimento(atendimento: any, agendamento: any): Date | undefined {
    if (atendimento?.dataAtendimento) {
      return new Date(atendimento.dataAtendimento);
    }
    if (agendamento?.data) {
      const data = new Date(agendamento.data);
      if (agendamento.hora) {
        const [horas, minutos] = agendamento.hora.split(':');
        data.setHours(parseInt(horas, 10), parseInt(minutos, 10));
      }
      return data;
    }
    return undefined;
  }

  private aplicarFiltros() {
    let filtrados = [...this.atendimentosPendentesOriginais];

    const filtros = this.filtrosForm.value;

    if (filtros.assistido) {
      filtrados = filtrados.filter((item) => item.assistido?.id === filtros.assistido);
    }

    if (filtros.agendamento) {
      filtrados = filtrados.filter((item) => item.agendamento?.id === filtros.agendamento);
    }

    if (filtros.dataInicio) {
      const dataInicio = new Date(filtros.dataInicio);
      filtrados = filtrados.filter(
        (item) => item.dataAtendimento && item.dataAtendimento >= dataInicio
      );
    }

    if (filtros.dataFim) {
      const dataFim = new Date(filtros.dataFim);
      filtrados = filtrados.filter(
        (item) => item.dataAtendimento && item.dataAtendimento <= dataFim
      );
    }

    if (filtros.presenca !== '' && filtros.presenca !== null && filtros.presenca !== undefined) {
      filtrados = filtrados.filter((item) => item.presenca === filtros.presenca);
    }

    if (filtros.status !== '' && filtros.status !== null && filtros.status !== undefined) {
      filtrados = filtrados.filter((item) => item.status === filtros.status);
    }

    // Atualizar a lista exibida
    this.atendimentosPendentes = filtrados;
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      assistido: [''],
      agendamento: [''],
      dataInicio: [''],
      dataFim: [''],
      presenca: [''],
      status: [''],
    });
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.pesquisarAtendimentos();
  }

  onAdd() {
    this.adicionarAtendimento();
  }

  onClear() {
    this.limparFiltros();
  }

  statusOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: Status.Ativo, label: 'Ativo' },
    { value: Status.Inativo, label: 'Inativo' },
  ];

  presencaOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: StatusAtendimentoEnum.PRESENTE, label: 'Presente' },
    { value: StatusAtendimentoEnum.FALTA, label: 'Falta' },
    { value: StatusAtendimentoEnum.FALTA_JUSTIFICADA, label: 'Falta Justificada' },
  ];

  tableColumns: TableColumn[] = [
    {
      key: 'dataAtendimento',
      label: 'Data/Hora',
      width: 'large',
      align: 'left',
      getCellValue: (row: AtendimentoPendente) => {
        if (row.dataAtendimento) {
          return `${row.dataAtendimento.toLocaleDateString()} ${row.dataAtendimento.toLocaleTimeString(
            'pt-BR',
            { hour: '2-digit', minute: '2-digit' }
          )}`;
        }
        if (row.agendamento?.data && row.agendamento?.hora) {
          return `${new Date(row.agendamento.data).toLocaleDateString()} ${row.agendamento.hora}`;
        }
        return 'Não informado';
      },
    },
    {
      key: 'assistido',
      label: 'Assistido',
      width: 'large',
      align: 'left',
      getCellValue: (row: AtendimentoPendente) => row.assistido?.nome || 'Não informado',
    },
    {
      key: 'agendamento',
      label: 'Agendamento',
      width: 'large',
      align: 'left',
      getCellValue: (row: AtendimentoPendente) => row.agendamento?.nome || 'Não informado',
    },
    {
      key: 'presenca',
      label: 'Presença',
      width: 'medium',
      align: 'center',
      getCellValue: (row: AtendimentoPendente) => {
        if (row.presenca === undefined || row.presenca === null) {
          return 'Pendente';
        }
        switch (row.presenca) {
          case StatusAtendimentoEnum.PRESENTE:
            return 'Presente';
          case StatusAtendimentoEnum.FALTA:
            return 'Falta';
          case StatusAtendimentoEnum.FALTA_JUSTIFICADA:
            return 'Falta Justificada';
          default:
            return 'Não informado';
        }
      },
    },
    {
      key: 'avaliacao',
      label: 'Avaliação',
      width: 'large',
      align: 'left',
      getCellValue: (row: AtendimentoPendente) =>
        row.avaliacao
          ? row.avaliacao.length > 50
            ? row.avaliacao.substring(0, 50) + '...'
            : row.avaliacao
          : 'Sem avaliação',
    },
    {
      key: 'status',
      label: 'Status',
      width: 'medium',
      align: 'center',
      getCellValue: (row: AtendimentoPendente) =>
        row.status === Status.Ativo ? 'Ativo' : 'Inativo',
    },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row: AtendimentoPendente) => this.editarAtendimento(row),
    },
    {
      icon: 'visibility',
      tooltip: 'Visualizar',
      color: 'primary',
      action: (row: AtendimentoPendente) => this.visualizarAtendimento(row),
    },
  ];

  adicionarAtendimento(): void {
    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '80%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: null,
      })
      .subscribe(() => this.pesquisarAtendimentos());
  }

  editarAtendimento(atendimentoPendente: AtendimentoPendente) {
    // Criar um objeto Atendimento a partir do AtendimentoPendente
    const atendimento = this.criarAtendimentoParaModal(atendimentoPendente);

    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '80%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: atendimento,
      })
      .subscribe(() => this.pesquisarAtendimentos());
  }

  visualizarAtendimento(atendimentoPendente: AtendimentoPendente) {
    // Criar um objeto Atendimento a partir do AtendimentoPendente
    const atendimento = this.criarAtendimentoParaModal(atendimentoPendente);

    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '80%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: atendimento,
        isVisualizacao: true,
      })
      .subscribe(() => this.pesquisarAtendimentos());
  }

  private criarAtendimentoParaModal(atendimentoPendente: AtendimentoPendente): Atendimento {
    return new Atendimento({
      id: atendimentoPendente.atendimento?.id || '',
      idAgendamento: atendimentoPendente.agendamento?.id || '',
      idAssistido: atendimentoPendente.assistido?.id || '',
      dataAtendimento: atendimentoPendente.dataAtendimento,
      presenca: atendimentoPendente.presenca,
      avaliacao: atendimentoPendente.avaliacao,
      status: atendimentoPendente.status || Status.Ativo,
      assistido: atendimentoPendente.assistido,
      agendamento: atendimentoPendente.agendamento,
    });
  }
}
