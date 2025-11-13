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

interface RelatorioPresenca {
  assistido: string;
  municipio: string;
  dataAtendimento: string;
  presente: string;
  profissional: string;
}

@Component({
  selector: 'app-relatorio-presenca',
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
  templateUrl: './relatorio-presenca.component.html',
  styleUrls: ['./relatorio-presenca.component.less'],
})
export class RelatorioPresencaComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioPresenca[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório de Presença',
      'Geração de listas de presença para entrega aos municípios'
    );
    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      municipio: [''],
      dataInicio: [''],
      dataFim: [''],
      profissional: [''],
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
        assistido: 'João Silva',
        municipio: 'Feliz',
        dataAtendimento: '2024-11-05',
        presente: 'Sim',
        profissional: 'Dr. Maria Santos',
      },
      {
        assistido: 'Ana Costa',
        municipio: 'Feliz',
        dataAtendimento: '2024-11-05',
        presente: 'Sim',
        profissional: 'Dr. Maria Santos',
      },
      {
        assistido: 'Carlos Souza',
        municipio: 'Vale Real',
        dataAtendimento: '2024-11-06',
        presente: 'Não',
        profissional: 'Dr. Pedro Lima',
      },
      {
        assistido: 'Maria Santos',
        municipio: 'Feliz',
        dataAtendimento: '2024-11-07',
        presente: 'Sim',
        profissional: 'Dra. Julia Ferreira',
      },
      {
        assistido: 'Pedro Oliveira',
        municipio: 'Linha Nova',
        dataAtendimento: '2024-11-08',
        presente: 'Sim',
        profissional: 'Dr. Maria Santos',
      },
    ];
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório de Presença gerado com sucesso!');
    // Aqui seria implementada a lógica real de geração do relatório
    console.log('Filtros aplicados:', this.filtrosForm.value);
  }

  municipiosOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Feliz' },
    { value: '2', label: 'Vale Real' },
    { value: '3', label: 'Linha Nova' },
  ];

  profissionaisOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Dr. Maria Santos' },
    { value: '2', label: 'Dr. Pedro Lima' },
    { value: '3', label: 'Dra. Julia Ferreira' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'assistido', label: 'Assistido', width: 'large', align: 'left' },
    { key: 'municipio', label: 'Município', width: 'medium', align: 'left' },
    { key: 'dataAtendimento', label: 'Data', width: 'medium', align: 'center' },
    {
      key: 'presente',
      label: 'Presente',
      width: 'small',
      align: 'center',
      getClass: (row) => (row.presente === 'Sim' ? 'status-ativo' : 'status-inativo'),
    },
    { key: 'profissional', label: 'Profissional', width: 'large', align: 'left' },
  ];
}
