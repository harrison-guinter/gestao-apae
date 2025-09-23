import { Component, OnInit } from '@angular/core';
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
import { ModalUsuariosComponent } from './modal-usuarios/modal-usuarios.component';
import { Usuario } from './usuario';
import { Roles } from '../auth/roles.enum';

@Component({
  selector: 'app-usuarios',
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
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.less'],
})
export class UsuariosComponent implements OnInit {
  protected perfisUsuario: SelectOption[] = [
    { value: Roles.COORDENADOR, label: 'Coordenador' },
    { value: Roles.PROFISSIONAL, label: 'Profissional' },
  ];

  protected filtrosForm!: UntypedFormGroup;

  usuarios: Usuario[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private modalService: ModalService
  ) {}

  ngOnInit() {
    // Atualizar informações da página
    this.pageInfoService.updatePageInfo('Usuários', 'Gerenciar usuários do sistema');

    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      name: [''],
      email: [''],
      perfil: [''],
      status: [''],
    });
  }

  limparFiltros() {
    this.filtrosForm.reset();
  }

  onAdd() {
    this.adicionarUsuario();
  }

  onClear() {
    this.limparFiltros();
  }

  perfilsUsuario: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: Roles.COORDENADOR, label: 'Coordenador' },
    { value: Roles.PROFISSIONAL, label: 'Profissional' },
  ];

  statusOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: true, label: 'Ativo' },
    { value: false, label: 'Inativo' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'name', label: 'Nome', width: 'large', align: 'left' },
    { key: 'email', label: 'E-mail', width: 'xlarge', align: 'left' },
    { key: 'perfil', label: 'Tipo', width: 'medium', align: 'center' },
    { key: 'status', label: 'Status', width: 'small', align: 'center' },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarUsuario(row),
    },
  ];

  adicionarUsuario() {

    this.modalService
      .openModal({
        component: ModalUsuariosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: false },
        element: null,
      })
      .subscribe();
  }

  editarUsuario(element: Usuario) {

    this.modalService
      .openModal({
        component: ModalUsuariosComponent,
        width: '60%',
        height: 'auto',
        disableClose: true,
        data: { isEdit: true },
        element: element,
      })
      .subscribe();
  }
}
