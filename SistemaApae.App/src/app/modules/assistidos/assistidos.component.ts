import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatBadgeModule } from '@angular/material/badge';
import { PageInfoService } from '../core/services/page-info.service';

@Component({
  selector: 'app-assistidos',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatBadgeModule,
  ],
  templateUrl: './assistidos.component.html',
  styleUrls: ['./assistidos.component.less'],
})
export class AssistidosComponent implements OnInit {
  constructor(private pageInfoService: PageInfoService) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo('Assistidos', 'Gerenciar assistidos cadastrados');
  }
  assistidos = [
    {
      id: 1,
      nome: 'Ana Clara Silva',
      idade: 12,
      responsavel: 'Maria Silva',
      telefone: '(11) 9999-8888',
      deficiencia: 'Síndrome de Down',
      ativo: true,
      atividades: ['Fisioterapia', 'Fonoaudiologia'],
    },
    {
      id: 2,
      nome: 'João Pedro Santos',
      idade: 8,
      responsavel: 'Carlos Santos',
      telefone: '(11) 8888-7777',
      deficiencia: 'Autismo',
      ativo: true,
      atividades: ['Terapia Ocupacional', 'Pedagogia'],
    },
    {
      id: 3,
      nome: 'Lucia Fernandes',
      idade: 15,
      responsavel: 'Roberto Fernandes',
      telefone: '(11) 7777-6666',
      deficiencia: 'Deficiência Intelectual',
      ativo: false,
      atividades: ['Fisioterapia'],
    },
  ];

  adicionarAssistido() {
    console.log('Adicionar novo assistido');
  }

  editarAssistido(id: number) {
    console.log(`Editar assistido com ID: ${id}`);
  }

  excluirAssistido(id: number) {
    console.log(`Excluir assistido com ID: ${id}`);
  }

  verDetalhes(id: number) {
    console.log(`Ver detalhes do assistido com ID: ${id}`);
  }
}
