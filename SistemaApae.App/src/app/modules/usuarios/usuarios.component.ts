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

interface Usuario {
  id: number;
  nome: string;
  email: string;
  tipo: 'Coordenador' | 'Profissional';
  especialidade: string;
  ativo: boolean;
}

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

  todosUsuarios: Usuario[] = [
    {
      id: 1,
      nome: 'Luana Marini',
      email: 'luana@apae.com',
      tipo: 'Coordenador',
      especialidade: '-',
      ativo: true,
    },
    {
      id: 2,
      nome: 'Dra. Maria Santos',
      email: 'maria@apae.com',
      tipo: 'Profissional',
      especialidade: 'Fonoaudióloga',
      ativo: true,
    },
    {
      id: 3,
      nome: 'Dr. João Silva',
      email: 'joao@apae.com',
      tipo: 'Profissional',
      especialidade: 'Fisioterapeuta',
      ativo: true,
    },
    {
      id: 4,
      nome: 'Dra. Ana Costa',
      email: 'ana@apae.com',
      tipo: 'Profissional',
      especialidade: 'Psicóloga',
      ativo: false,
    },
    {
      id: 5,
      nome: 'Dr. Carlos Oliveira',
      email: 'carlos@apae.com',
      tipo: 'Profissional',
      especialidade: 'Terapeuta Ocupacional',
      ativo: true,
    },
    {
      id: 6,
      nome: 'Luana Marini',
      email: 'luana@apae.com',
      tipo: 'Coordenador',
      especialidade: '-',
      ativo: true,
    },
    {
      id: 7,
      nome: 'Dra. Maria Santos',
      email: 'maria@apae.com',
      tipo: 'Profissional',
      especialidade: 'Fonoaudióloga',
      ativo: true,
    },
    {
      id: 8,
      nome: 'Dr. João Silva',
      email: 'joao@apae.com',
      tipo: 'Profissional',
      especialidade: 'Fisioterapeuta',
      ativo: true,
    },
    {
      id: 9,
      nome: 'Dra. Ana Costa',
      email: 'ana@apae.com',
      tipo: 'Profissional',
      especialidade: 'Psicóloga',
      ativo: false,
    },
    {
      id: 10,
      nome: 'Dr. Carlos Oliveira',
      email: 'carlos@apae.com',
      tipo: 'Profissional',
      especialidade: 'Terapeuta Ocupacional',
      ativo: true,
    },
  ];

  usuarios: Usuario[] = [];

  constructor(private formBuilder: UntypedFormBuilder, private pageInfoService: PageInfoService) {}

  ngOnInit() {
    // Atualizar informações da página
    this.pageInfoService.updatePageInfo('Usuários', 'Gerenciar usuários do sistema');

    this.initFiltrosForm();
    this.aplicarFiltros();

    this.filtrosForm.valueChanges.subscribe(() => {
      this.aplicarFiltros();
    });
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      nome: [''],
      email: [''],
      tipo: [''],
      status: [''],
    });
  }

  aplicarFiltros() {
    const filtros = this.filtrosForm.value;

    this.usuarios = this.todosUsuarios.filter((usuario) => {
      const nomeMatch =
        !filtros.nome || usuario.nome.toLowerCase().includes(filtros.nome.toLowerCase());

      const emailMatch =
        !filtros.email || usuario.email.toLowerCase().includes(filtros.email.toLowerCase());

      const tipoMatch = !filtros.tipo || usuario.tipo === filtros.tipo;

      const statusMatch =
        !filtros.status ||
        (filtros.status === 'ativo' && usuario.ativo) ||
        (filtros.status === 'inativo' && !usuario.ativo);

      return nomeMatch && emailMatch && tipoMatch && statusMatch;
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

  tiposUsuario: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: 'Coordenador', label: 'Coordenador' },
    { value: 'Profissional', label: 'Profissional' },
  ];

  statusOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: 'ativo', label: 'Ativo' },
    { value: 'inativo', label: 'Inativo' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Nome', width: 'large', align: 'left' },
    { key: 'email', label: 'E-mail', width: 'xlarge', align: 'left' },
    { key: 'tipo', label: 'Tipo', width: 'medium', align: 'center' },
    { key: 'status', label: 'Status', width: 'small', align: 'center' },
  ];

  tableActions: TableAction[] = [
    {
      icon: 'edit',
      tooltip: 'Editar',
      color: 'primary',
      action: (row) => this.editarUsuario(row.id),
    },
  ];

  adicionarUsuario() {
    console.log('Adicionar novo usuário');
  }

  editarUsuario(id: number) {
    console.log(`Editar usuário com ID: ${id}`);
  }

  excluirUsuario(id: number) {
    console.log(`Excluir usuário com ID: ${id}`);
  }

  reativarUsuario(id: number) {
    console.log(`Reativar usuário com ID: ${id}`);
  }

  onRowClick(row: any) {
    console.log('Linha clicada:', row);
  }

  onActionClick(event: { action: string; row: any }) {
    console.log('Ação executada:', event.action, 'Linha:', event.row);
  }
}
