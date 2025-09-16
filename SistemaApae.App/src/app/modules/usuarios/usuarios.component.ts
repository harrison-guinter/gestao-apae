import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule } from '@angular/material/card';
import { TableComponent, TableColumn, TableAction } from '../core/table/table.component';
import { PageInfoService } from '../core/services/page-info.service';

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
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCardModule,
    TableComponent,
  ],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.less'],
})
export class UsuariosComponent implements OnInit {
  filtrosForm!: UntypedFormGroup;

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

  tiposUsuario = [
    { value: '', label: 'Todos os tipos' },
    { value: 'Coordenador', label: 'Coordenador' },
    { value: 'Profissional', label: 'Profissional' },
  ];

  statusOptions = [
    { value: '', label: 'Todos os status' },
    { value: 'ativo', label: 'Ativo' },
    { value: 'inativo', label: 'Inativo' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Nome' },
    { key: 'email', label: 'E-mail' },
    { key: 'tipo', label: 'Tipo' },
    { key: 'status', label: 'Status' },
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
