import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { Router } from '@angular/router';
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
import { Assistido, StatusAssistidoEnum } from './assistido';
import { AssistidoService, AssistidoFiltro } from './assistido.service';
import { NotificationService } from '../core/notification/notification.service';

@Component({
  selector: 'app-assistidos',
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
  templateUrl: './assistidos.component.html',
  styleUrls: ['./assistidos.component.less'],
})
export class AssistidosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  assistidos: Assistido[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private router: Router,
    private assistidoService: AssistidoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Assistidos', 'Gerenciar assistidos do sistema');

    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      nome: [''],
      cpf: [''],
      status: [StatusAssistidoEnum.ATIVO],
    });
    this.pesquisarAssistidos();
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.filtrosForm.patchValue({ status: StatusAssistidoEnum.ATIVO });
    this.pesquisarAssistidos();
  }

  onAdd() {
    this.adicionarAssistido();
  }

  onClear() {
    this.limparFiltros();
  }

  pesquisarAssistidos() {
    const filtros: AssistidoFiltro = {
      nome: this.filtrosForm.get('nome')?.value || undefined,
      cpf: this.filtrosForm.get('cpf')?.value || undefined,
      status:
        this.filtrosForm.get('status')?.value !== ''
          ? this.filtrosForm.get('status')?.value
          : undefined,
    };

    this.assistidoService.listarAssistidos(filtros).subscribe((assistidos) => {
      this.assistidos = assistidos;
      if (assistidos.length === 0) {
        this.notificationService.showInfo('Nenhum assistido encontrado com os filtros aplicados');
      }
    });
  }

  statusOptions: SelectOption[] = [
    { value: StatusAssistidoEnum.ATIVO, label: 'Ativo' },
    { value: StatusAssistidoEnum.INATIVO, label: 'Inativo' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Nome', width: 'xlarge', align: 'left' },
    { key: 'cpf', label: 'CPF', width: 'medium', align: 'left' },
    {
      key: 'dataNascimento',
      label: 'Data Nasc.',
      width: 'medium',
      align: 'center',
      getCellValue: (row) =>
        row.dataNascimento ? new Date(row.dataNascimento).toLocaleDateString('pt-BR') : '-',
    },
    {
      key: 'status',
      label: 'Status',
      width: 'small',
      align: 'center',
      getCellValue: (row) => (row.status === StatusAssistidoEnum.ATIVO ? 'Ativo' : 'Inativo'),
      getClass: (row) =>
        row.status === StatusAssistidoEnum.ATIVO ? 'status-ativo' : 'status-inativo',
    },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarAssistido(row),
    },
  ];

  adicionarAssistido() {
    this.router.navigate(['/home/assistidos/cadastro']);
  }

  editarAssistido(assistido: Assistido) {
    this.router.navigate(['/home/assistidos/cadastro', assistido.id]);
  }
}
