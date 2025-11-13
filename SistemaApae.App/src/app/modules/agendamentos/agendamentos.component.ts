import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TableComponent, TableColumn, TableAction } from '../core/table/table.component';
import { SelectComponent, SelectOption } from '../core/select/select.component';
import { InputComponent } from '../core/input/input.component';
import { PageInfoService } from '../core/services/page-info.service';
import { FiltersContainerComponent } from '../core/filters-container/filters-container.component';
import { ModalService } from '../core/services/modal.service';
import { Agendamento, DiaDaSemana, TipoRecorrencia } from './agendamento';
import { ModalAgendamentosComponent } from './modal-agendamentos/modal-agendamentos.component';
import { AgendamentoService } from './agendamento.service';
import { AgendamentoFiltro } from './agendamento.service';
import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';
import { Usuario } from '../usuarios/usuario';
import { Roles } from '../auth/roles.enum';
import { UsuarioService } from '../usuarios/usuario.service';
import { map, Observable } from 'rxjs';
import { AutocompleteComponent } from '../core/autocomplete/autocomplete.component';
import { DatepickerComponent } from '../core/date/datepicker/datepicker.component';
import { RangePickerComponent } from '../core/date/rangepicker/rangepicker.component';
import { NotificationService } from '../core/notification/notification.service';
import { AssistidoService } from '../assistidos/assistido.service';
import { DateUtils } from '../core/date/date-utils';

@Component({
  selector: 'app-agendamentos',
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
    InputComponent,
    FiltersContainerComponent,
    AutocompleteComponent,
    RangePickerComponent,
  ],
  templateUrl: './agendamentos.component.html',
  styleUrls: ['./agendamentos.component.less'],
})
export class AgendamentosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private agendamentoService = inject(AgendamentoService);

  private usuarioService = inject(UsuarioService);

  private assistidoService: AssistidoService = inject(AssistidoService);

  protected agendamentos: Agendamento[] = [];

  profissionalOptions$: Observable<SelectOption[]> = this.buscarProfissionais().pipe(
    map((users) =>
      users.map((user) => ({
        value: user,
        label: user.nome,
      }))
    )
  );

  assistidosOptions$: Observable<SelectOption[]> = this.assistidoService.listarAssistidos({}).pipe(
    map((assistidos) =>
      assistidos.map((assistido) => ({
        value: assistido,
        label: assistido.nome,
      }))
    )
  );

  isLoadingUsers: boolean = false;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Agendamentos', 'Gerenciar agendamentos do sistema');

    this.initFiltrosForm();
  }

  pesquisarAgendamentos() {
    const filtros: AgendamentoFiltro = this.valueFromForm();

    this.agendamentoService.listarAgendamentos(filtros).subscribe({
      next: (response) => {
        this.agendamentos = response;
      },
      error: (error) => {
        this.notificationService.fail('Erro ao pesquisar agendamentos');
        console.error(error);
      },
    });
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      idProfissional: ['', AutocompleteComponent.selectOptionValidator],
      idAssistido: ['', AutocompleteComponent.selectOptionValidator],
      dataAgendamentoInicio: [''],
      dataAgendamentoFim: [''],
      status: [''],
    });
  }

  limparFiltros() {
    this.filtrosForm.reset();
  }

  onAdd() {
    this.adicionarAgendamento();
  }

  onClear() {
    this.limparFiltros();
  }

  statusOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: Status.Ativo, label: 'Ativo' },
    { value: Status.Inativo, label: 'Inativo' },
  ];

  recorrenciaOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: TipoRecorrencia.NENHUM, label: 'Sem recorrência' },
    { value: TipoRecorrencia.SEMANAL, label: 'Semanal' },
  ];

  diaDaSemanaOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: DiaDaSemana.SEGUNDA, label: 'Segunda' },
    { value: DiaDaSemana.TERCA, label: 'Terça' },
    { value: DiaDaSemana.QUARTA, label: 'Quarta' },
    { value: DiaDaSemana.QUINTA, label: 'Quinta' },
    { value: DiaDaSemana.SEXTA, label: 'Sexta' },
    { value: DiaDaSemana.SABADO, label: 'Sábado' },
  ];

  tableColumns: TableColumn[] = [
    {
      key: 'data',
      label: 'Data',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.tipoRecorrencia == TipoRecorrencia.NENHUM ? DateUtils.fromDbToDisplay(row.dataAgendamento) : '-',
    },
    {
      key: 'profissional',
      label: 'Profissional',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.profissional.nome,
    },
    {
      key: 'assistidos',
      label: 'Assistidos',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.assistidos.map((a: Assistido) => a.nome).join(', '),
    },
    {
      key: 'recorrencia',
      label: 'Recorrência',
      width: 'large',
      align: 'left',
      getCellValue: (row) =>
        this.recorrenciaOptions.find((item) => item.value == row.tipoRecorrencia)?.label || '',
    },
    {
      key: 'diaDaSemana',
      label: 'Dia da semana',
      width: 'large',
      align: 'left',
      getCellValue: (row) =>
        row.tipoRecorrencia == TipoRecorrencia.SEMANAL
          ? this.diaDaSemanaOptions.find((item) => item.value == row.diaSemana)?.label
          : '-',
    },
    {
      key: 'status',
      label: 'Status',
      width: 'large',
      align: 'left',
      getCellValue: (row) => (row.status === Status.Ativo ? 'Ativo' : 'Inativo'),
    },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarAgendamento(row),
    },
    {
      icon: 'visibility',
      tooltip: 'Visualizar',
      color: 'primary',
      action: (row) => this.visualizarAgendamento(row),
    },
  ];

  adicionarAgendamento(): void {
    this.modalService
      .openModal({
        component: ModalAgendamentosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: null,
      })
      .subscribe((atualizar) => atualizar ?? this.pesquisarAgendamentos());
  }

  editarAgendamento(element: Agendamento) {
    this.modalService
      .openModal({
        component: ModalAgendamentosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
      })
      .subscribe((atualizar) => atualizar ?? this.pesquisarAgendamentos());
  }

  visualizarAgendamento(element: Agendamento) {
    this.modalService
      .openModal({
        component: ModalAgendamentosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
        isVisualizacao: true,
      })
      .subscribe();
  }

  private buscarProfissionais(): Observable<Usuario[]> {
    return this.usuarioService
      .filtrarUsuarios({ perfil: Roles.PROFISSIONAL, status: Status.Ativo })
      .pipe(
        map((users) => {
          return users.map((u) => new Usuario(u));
        })
      );
  }

  valueFromForm(): AgendamentoFiltro {
    const filtros = this.filtrosForm.value;

    if (filtros.dataAgendamentoFim) filtros.dataAgendamentoFim = DateUtils.fromFieldToDb(filtros.dataAgendamentoFim)
    if (filtros.dataAgendamentoInicio) filtros.dataAgendamentoInicio = DateUtils.fromFieldToDb(filtros.dataAgendamentoInicio)
    if (filtros.idAssistido) filtros.idAssistido = filtros.idAssistido.value.id
    if (filtros.idProfissional) filtros.idProfissional = filtros.idProfissional.value.id

    return { ...filtros };
  }
}
