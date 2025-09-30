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
import { Convenio } from './convenio';
import { ModalConveniosComponent } from './modal-convenios/modal-convenios.component';
import { ConvenioService } from './convenio.service';
import { NotificationService } from '../core/notification/notification.service';
import { ConvenioFiltro } from './convenio.service';

@Component({
  selector: 'app-convenios',
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
  templateUrl: './convenios.component.html',
  styleUrls: ['./convenios.component.less'],
})
export class ConveniosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;

  private convenioService = inject(ConvenioService);

  protected convenios: Convenio[] = []

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Convênios', 'Gerenciar convênios do sistema');

    this.initFiltrosForm();
    this.pesquisarConvenios()
  }

    pesquisarConvenios() {
      const filtros: ConvenioFiltro = this.filtrosForm.value;
  
       this.convenioService.listarConvenios(filtros).subscribe((convenios) => {
        this.convenios = convenios;

        if (convenios.length === 0) {
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
    this.adicionarConvenio();
  }

  onClear() {
    this.limparFiltros();
  }

  statusOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: true, label: 'Ativo' },
    { value: false, label: 'Inativo' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Nome', width: 'large', align: 'left' },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarConvenio(row),
    },
     {
      icon: 'visibility',
      tooltip: 'Visualizar',
      color: 'primary',
      action: (row) => this.visualizarConvenio(row),
    },
  ];

  adicionarConvenio(): void {

    this.modalService
      .openModal({
        component: ModalConveniosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: null,
      })
      .subscribe();
  }

  editarConvenio(element: Convenio) {
    this.modalService
      .openModal({
        component: ModalConveniosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
      })
      .subscribe(() => this.pesquisarConvenios());
  }

  visualizarConvenio(element: Convenio) {
    this.modalService
      .openModal({
        component: ModalConveniosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
        isVisualizacao: true
      })
      .subscribe(() => this.pesquisarConvenios());
  }
}
