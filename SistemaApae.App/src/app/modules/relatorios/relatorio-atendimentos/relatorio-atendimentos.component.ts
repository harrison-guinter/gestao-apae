import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { TableComponent, TableColumn } from '../../core/table/table.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { InputComponent } from '../../core/input/input.component';
import { PageInfoService } from '../../core/services/page-info.service';
import { FiltersContainerComponent } from '../../core/filters-container/filters-container.component';
import { NotificationService } from '../../core/notification/notification.service';

interface RelatorioAssistidosAtendidos {
  diaSemana: string;
  profissional: string;
  quantidadeAssistidos: number;
  municipio: string;
}

@Component({
  selector: 'app-relatorio-assistidos-atendidos',
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
  templateUrl: './relatorio-atendimentos.component.html',
  styleUrls: ['./relatorio-atendimentos.component.less'],
})
export class RelatorioAtendimentosComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioAssistidosAtendidos[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório de Atendimentos',
      'Quantidade de assistidos atendidos por dia da semana'
    );
    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      diaSemana: [''],
      profissional: [''],
      dataInicio: [''],
      dataFim: [''],
    });
    this.pesquisar();
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.pesquisar();
  }

  pesquisar() {
    // Mock de dados para preview
    this.relatorioData = [
      {
        diaSemana: 'Segunda-feira',
        profissional: 'Dr. Maria Santos',
        quantidadeAssistidos: 12,
        municipio: 'Feliz',
      },
      {
        diaSemana: 'Terça-feira',
        profissional: 'Dr. Pedro Lima',
        quantidadeAssistidos: 8,
        municipio: 'Vale Real',
      },
      {
        diaSemana: 'Quarta-feira',
        profissional: 'Dra. Julia Ferreira',
        quantidadeAssistidos: 15,
        municipio: 'Feliz',
      },
      {
        diaSemana: 'Quinta-feira',
        profissional: 'Dr. Maria Santos',
        quantidadeAssistidos: 10,
        municipio: 'Linha Nova',
      },
      {
        diaSemana: 'Sexta-feira',
        profissional: 'Dr. Pedro Lima',
        quantidadeAssistidos: 7,
        municipio: 'Vale Real',
      },
    ];
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório de Assistidos Atendidos gerado com sucesso!');
    // Aqui seria implementada a lógica real de geração do relatório
    console.log('Filtros aplicados:', this.filtrosForm.value);
  }

  diasSemanaOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Segunda-feira' },
    { value: '2', label: 'Terça-feira' },
    { value: '3', label: 'Quarta-feira' },
    { value: '4', label: 'Quinta-feira' },
    { value: '5', label: 'Sexta-feira' },
  ];

  profissionaisOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Dr. Maria Santos' },
    { value: '2', label: 'Dr. Pedro Lima' },
    { value: '3', label: 'Dra. Julia Ferreira' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'diaSemana', label: 'Dia da Semana', width: 'large', align: 'left' },
    { key: 'profissional', label: 'Profissional', width: 'large', align: 'left' },
    { key: 'quantidadeAssistidos', label: 'Qtd. Assistidos', width: 'medium', align: 'center' },
    { key: 'municipio', label: 'Município', width: 'medium', align: 'left' },
  ];
}
