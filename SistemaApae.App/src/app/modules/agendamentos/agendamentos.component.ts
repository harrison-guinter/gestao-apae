import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
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
import { Agendamento } from './agendamento';
import { ModalAgendamentosComponent } from './modal-agendamentos/modal-agendamentos.component';
import { AgendamentoService } from './agendamento.service';
import { NotificationService } from '../core/notification/notification.service';
import { AgendamentoFiltro } from './agendamento.service';
import { Status } from '../core/enum/status.enum';

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
  ],
  templateUrl: './agendamentos.component.html',
  styleUrls: ['./agendamentos.component.less'],
})
export class AgendamentosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private agendamentoService = inject(AgendamentoService);

  protected agendamentos: Agendamento[] = []

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Agendamentos', 'Gerenciar convênios do sistema');

    this.initFiltrosForm();
    this.pesquisarAgendamentos()
  }

    pesquisarAgendamentos() {
      const filtros: AgendamentoFiltro = this.filtrosForm.value;
  
       this.agendamentoService.listarAgendamentos(filtros).subscribe((agendamentos) => {
        this.agendamentos = agendamentos;

        if (agendamentos.length === 0) {
          this.notificationService.showInfo('Nenhum convênio encontrado com os filtros aplicados');
        }
      });
    }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
        Nome: [],
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

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Nome', width: 'large', align: 'left' },
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
        isVisualizacao: true
      })
      .subscribe(() => this.pesquisarAgendamentos());
  }
}
