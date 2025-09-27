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
import { UsuarioService, UsuarioFiltro } from './usuario.service';
import { NotificationService } from '../core/notification/notification.service';

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
  protected filtrosForm!: UntypedFormGroup;

  usuarios: Usuario[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private modalService: ModalService,
    private usuarioService: UsuarioService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Usuários', 'Gerenciar usuários do sistema');

    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      nome: [''],
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

  pesquisarUsuarios() {
    const filtros: UsuarioFiltro = {
      nome: this.filtrosForm.get('nome')?.value || undefined,
      email: this.filtrosForm.get('email')?.value || undefined,
      perfil: this.filtrosForm.get('perfil')?.value || undefined,
      status:
        this.filtrosForm.get('status')?.value !== ''
          ? this.filtrosForm.get('status')?.value
          : undefined,
    };

    // Com o interceptor, agora podemos usar subscribe simples
    this.usuarioService.listarUsuarios(filtros).subscribe((usuarios) => {
      this.usuarios = usuarios;
      if (usuarios.length === 0) {
        this.notificationService.showInfo('Nenhum usuário encontrado com os filtros aplicados');
      }
    });
  }

  perfisUsuario: SelectOption[] = [
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
    { key: 'nome', label: 'Nome', width: 'large', align: 'left' },
    { key: 'email', label: 'E-mail', width: 'xlarge', align: 'left' },
    {
      key: 'perfil',
      label: 'Tipo',
      width: 'medium',
      align: 'left',
      getCellValue: (row) =>
        row.perfil === Roles.PROFISSIONAL
          ? 'Profissional'
          : row.perfil === Roles.COORDENADOR
          ? 'Coordenador'
          : row.perfil,
    },
    {
      key: 'status',
      label: 'Status',
      width: 'small',
      align: 'center',
      getCellValue: (row) => (row.status ? 'Ativo' : 'Inativo'),
      getClass: (row) => (row.status ? 'status-ativo' : 'status-inativo'),
    },
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
