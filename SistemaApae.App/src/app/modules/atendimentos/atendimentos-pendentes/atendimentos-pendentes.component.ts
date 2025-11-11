import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Usuario } from '../../usuarios/usuario';
import { Assistido } from '../../assistidos/assistido';
import { TableAction, TableColumn, TableComponent } from '../../core/table/table.component';
import { AutocompleteComponent } from '../../core/autocomplete/autocomplete.component';
import { FiltersContainerComponent } from '../../core/filters-container/filters-container.component';
import { AgendamentoService } from '../../agendamentos/agendamento.service';
import { DiaDaSemana, TipoRecorrencia } from '../../agendamentos/agendamento';
import { SelectOption } from '../../core/select/select.component';
import { NotificationService } from '../../core/notification/notification.service';
import { PageInfoService } from '../../core/services/page-info.service';
import { ModalService } from '../../core/services/modal.service';
import { ModalAtendimentosComponent } from '../modal-atendimentos/modal-atendimentos.component';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';

interface AtendimentoPendente {
  assistido: Assistido;
  dataAgendamento: Date;
  horarioAgendamento: string;
  tipoRecorrencia: TipoRecorrencia;
  profissional: Usuario;
}

@Component({
  selector: 'app-atendimentos-pendentes',
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
    FiltersContainerComponent,
    AutocompleteComponent,
    DatepickerComponent
  ],
  templateUrl: './atendimentos-pendentes.component.html',
  styleUrls: ['./atendimentos-pendentes.component.less'],
})
export class AtendimentosPendentesComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private agendamentoService = inject(AgendamentoService);

  protected atendimentosPendentes: AtendimentoPendente[] = [];

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

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService,
    private modalService: ModalService
  ) { }

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Agendamentos', 'Gerenciar convênios do sistema');

    this.initFiltrosForm();
  }


  pesquisarAgendamentos() {
    const filtros = this.valueFromForm();

    console.log(filtros);
    this.agendamentoService.listarAgendamentosPorProfissional(filtros.idProfissional, filtros.dataAgendamento).subscribe({
      next: (response) => {
        this.atendimentosPendentes = []
        response.forEach((agendamento) => {
          agendamento.assistidos.forEach((assistido) => {
            this.atendimentosPendentes.push({
              assistido: assistido,
              dataAgendamento: agendamento.dataAgendamento,
              horarioAgendamento: agendamento.horarioAgendamento,
              profissional: agendamento.profissional,
              tipoRecorrencia: agendamento.tipoRecorrencia,
            });
        })
        })
      },
      error: (error) => {
        this.notificationService.fail('Erro ao pesquisar atendimentos pendentes');
        console.error(error);
      }
    });

  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      dataAgendamento: [new Date(), Validators.required],
    });
  }

  limparFiltros() {
    this.filtrosForm.reset();
  }


  onClear() {
    this.limparFiltros();
  }

  tableColumns: TableColumn[] = [
    {
      key: 'data',
      label: 'Data',
      width: 'large',
      align: 'left',
      getCellValue: (row) => row.tipoRecorrencia == TipoRecorrencia.NENHUM ? new Date(row.dataAgendamento).toLocaleDateString() : '-',
    },
    { key: 'hora', label: 'Horário', width: 'large', align: 'left', getCellValue: (row) => row.horarioAgendamento.slice(0, 5), },
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
      getCellValue: (row) => row.assistido.nome,
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
         row.tipoRecorrencia == TipoRecorrencia.SEMANAL ? this.diaDaSemanaOptions.find((item) => item.value == row.diaSemana)?.label :  '-',
    }
  ];

  tableActions: TableAction[] = [
    {
      icon: 'assignment_add',
      tooltip: 'Atender',
      color: 'primary',
      action: (row) => this.realizarAtendimento(row),
    }
  ];



  realizarAtendimento(element: AtendimentoPendente) {
    this.modalService
      .openModal({
        component: ModalAtendimentosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
      })
      .subscribe((atualizar) => atualizar ?? this.pesquisarAgendamentos());
  }

  valueFromForm(): { dataAgendamento: string; idProfissional: string } {
    const filtros = this.filtrosForm.value;

    if (filtros.dataAgendamento) filtros.dataAgendamento = new Date(filtros.dataAgendamento).toISOString().slice(0, 10)

    filtros.idProfissional = Usuario.getCurrentUser().id;

    return { ...filtros };
  }


}
