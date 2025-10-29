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
import { DatepickerComponent as DateComponent } from '../../core/date/datepicker/datepicker.component';

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
  templateUrl: './atendimentos-realizados.component.html',
  styleUrls: ['./atendimentos-realizados.component.less'],
})
export class AtendimentosRealizadosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private atendimentoService = inject(AtendimentoService);
  private assistidoService = inject(AssistidoService);
  private agendamentoService = inject(AgendamentoService);

  protected atendimentos: Atendimento[] = [];

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
          label: `${agendamento.nome} - ${agendamento.data.toLocaleDateString()} ${
            agendamento.hora
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
    const filtros: AtendimentoFiltro = {
      idAssistido: this.filtrosForm.value.assistido || undefined,
      idAgendamento: this.filtrosForm.value.agendamento || undefined,
      dataInicioAtendimento: this.filtrosForm.value.dataInicio || undefined,
      dataFimAtendimento: this.filtrosForm.value.dataFim || undefined,
      presenca:
        this.filtrosForm.value.presenca !== '' ? this.filtrosForm.value.presenca : undefined,
      status: this.filtrosForm.value.status !== '' ? this.filtrosForm.value.status : undefined,
    };

    this.atendimentoService.listarAtendimentos(filtros).subscribe({
      next: (response) => {
        this.atendimentos = response;
      },
      error: (error) => {
        console.error('Erro ao pesquisar atendimentos:', error);
      },
    });
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
      getCellValue: (row) =>
        row.dataAtendimento
          ? `${row.dataAtendimento.toLocaleDateString()} ${row.dataAtendimento.toLocaleTimeString(
              'pt-BR',
              { hour: '2-digit', minute: '2-digit' }
            )}`
          : 'Não informado',
    },
    {
      key: 'assistido',
      label: 'Assistido',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.assistido?.nome || 'Não informado',
    },
    {
      key: 'agendamento',
      label: 'Agendamento',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.agendamento?.nome || 'Não informado',
    },
    {
      key: 'presenca',
      label: 'Presença',
      width: 'medium',
      align: 'center',
      getCellValue: (row) => row.presencaTexto,
    },
    {
      key: 'avaliacao',
      label: 'Avaliação',
      width: 'large',
      align: 'left',
      getCellValue: (row) =>
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
      getCellValue: (row) => row.statusTexto,
    },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarAtendimento(row),
    },
    {
      icon: 'visibility',
      tooltip: 'Visualizar',
      color: 'primary',
      action: (row) => this.visualizarAtendimento(row),
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

  editarAtendimento(element: Atendimento) {
    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '80%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
      })
      .subscribe(() => this.pesquisarAtendimentos());
  }

  visualizarAtendimento(element: Atendimento) {
    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '80%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: element,
        isVisualizacao: true,
      })
      .subscribe(() => this.pesquisarAtendimentos());
  }
}
