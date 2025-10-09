import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
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
import { NotificationService } from '../core/notification/notification.service';
import { AgendamentoFiltro } from './agendamento.service';
import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';
import { StatusUsuarioEnum, Usuario } from '../usuarios/usuario';
import { Roles } from '../auth/roles.enum';
import { UsuarioService } from '../usuarios/usuario.service';
import {
  debounceTime,
  distinctUntilChanged,
  filter,
  map,
  Observable,
  of,
  switchMap,
  tap,
  withLatestFrom,
} from 'rxjs';
import { AutocompleteComponent } from '../core/autocomplete/autocomplete.component';

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
  ],
  templateUrl: './agendamentos.component.html',
  styleUrls: ['./agendamentos.component.less'],
})
export class AgendamentosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private agendamentoService = inject(AgendamentoService);

  private usuarioService = inject(UsuarioService);

  protected agendamentos: Agendamento[] = [];

profissionalOptions: Observable<SelectOption[]> = this.buscarProfissionais().pipe(
  map((users) =>
    users.map((user) => ({
      value: user, // objeto completo
      label: user.nome,
    }))
  )
);


  isLoadingUsers: boolean = false;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Agendamentos', 'Gerenciar convênios do sistema');

    this.initFiltrosForm();
    this.pesquisarAgendamentos();
  }

  pesquisarAgendamentos() {
    const filtros: AgendamentoFiltro = this.filtrosForm.value;
    // Exemplo de dados mockados para teste
    const agendamentosExemplo: Agendamento[] = [
      new Agendamento(
        'AG001',
        'Sessão de Fisioterapia',
        'Primeira sessão de avaliação motora',
        Status.Ativo,
        [
          {
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
          } as unknown as Assistido,
        ],
        new Usuario(
          'U1',
          'Dra. Paula Andrade',
          'paula@apae.org',
          Roles.PROFISSIONAL,
          StatusUsuarioEnum.ATIVO,
          'Fisioterapia',
          'Atende público infantil',
          'CREFITO 12345',
          '(49) 97777-3333'
        ),
        TipoRecorrencia.SEMANAL,
        new Date('2025-10-07'),
        '09:00',
        DiaDaSemana.TERCA
      ),

      new Agendamento(
        'AG002',
        'Atendimento Psicológico',
        'Sessão de acompanhamento emocional',
        Status.Ativo,
        [
          {
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
          } as unknown as Assistido,
        ],
        new Usuario(
          'U2',
          'Dr. Ricardo Menezes',
          'ricardo@apae.org',
          Roles.PROFISSIONAL,
          StatusUsuarioEnum.ATIVO,
          'Psicologia',
          'Especialista em comportamento infantil',
          'CRP 06/98765',
          '(49) 96666-4444'
        ),
        TipoRecorrencia.SEMANAL,
        new Date('2025-10-08'),
        '14:00',
        DiaDaSemana.QUARTA
      ),

      new Agendamento(
        'AG003',
        'Reunião Interdisciplinar',
        'Reunião entre profissionais e familiares para acompanhamento de casos',
        Status.Inativo,
        [
          {
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
          } as unknown as Assistido,
          {
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
          } as unknown as Assistido,
        ],
        new Usuario(
          'U2',
          'Dr. Ricardo Menezes',
          'ricardo@apae.org',
          Roles.PROFISSIONAL,
          StatusUsuarioEnum.ATIVO,
          'Psicologia',
          'Especialista em comportamento infantil',
          'CRP 06/98765',
          '(49) 96666-4444'
        ),
        TipoRecorrencia.NENHUM,
        new Date('2025-10-10'),
        '10:30',
        undefined
      ),
    ];

    // Simular chamada ao serviço
    this.agendamentos = agendamentosExemplo;
    /*
      this.agendamentoService.pesquisar(filtros).subscribe({
        next: (response) => {
          this.agendamentos = response;
        },
        error: (error) => {
          this.notificationService.error('Erro ao pesquisar agendamentos');
          console.error(error);
        }
      });
      */
  }

  initFiltrosForm() {
 this.filtrosForm = this.formBuilder.group({
  profissional: [null], // agora armazena o objeto SelectOption<Usuario>
  assistidoId: [''],
  data: [''],
  recorrencia: [null],
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
      getCellValue: (row) => row.data.toLocaleDateString(),
    },
    { key: 'hora', label: 'Horário', width: 'large', align: 'left' },
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
        this.diaDaSemanaOptions.find((item) => item.value == row.diaDaSemana)?.label || '',
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
      .subscribe(() => this.pesquisarAgendamentos());
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
      .subscribe(() => this.pesquisarAgendamentos());
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
      .subscribe(() => this.pesquisarAgendamentos());
  }

  private buscarProfissionais(): Observable<Usuario[]> {
    return this.usuarioService.listarUsuarios().pipe(
      map((users) => {
        return users
          .map((u) => new Usuario(u))
          .filter((u) => u.hasRole(Roles.PROFISSIONAL) && u.status === StatusUsuarioEnum.ATIVO);
      })
    );
  }

  onProfissionalSelecionado(option: SelectOption) {
  console.log('Profissional selecionado:', option.value);
  // Exemplo: atualizar outro campo se precisar
}

}
