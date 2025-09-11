import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './usuarios.component.html',
  styleUrls: ['./usuarios.component.less'],
})
export class UsuariosComponent {
  usuarios = [
    { id: 1, nome: 'João Silva', email: 'joao@email.com', ativo: true },
    { id: 2, nome: 'Maria Santos', email: 'maria@email.com', ativo: true },
    { id: 3, nome: 'Pedro Costa', email: 'pedro@email.com', ativo: false },
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
}
