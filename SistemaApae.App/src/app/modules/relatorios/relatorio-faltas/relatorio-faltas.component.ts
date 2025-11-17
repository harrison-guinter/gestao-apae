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

interface RelatorioFalta {
  assistido: string;
  profissional: string;
  data: string;
  municipio: string;
  observacao: string;
}

@Component({
  selector: 'app-relatorio-faltas',
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
  templateUrl: './relatorio-faltas.component.html',
  styleUrls: ['./relatorio-faltas.component.less'],
})
export class RelatorioFaltasComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioFalta[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório de Faltas',
      'Registro de ausências por assistido'
    );
    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      profissional: [''],
      dataInicio: [''],
      dataFim: [''],
      municipio: [''],
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
        profissional: 'Dr. Maria Santos',
        data: '2024-11-05',
        municipio: 'Feliz',
        observacao: 'Doença',
      },
      {
        assistido: 'Ana Costa',
        profissional: 'Dr. Pedro Lima',
        data: '2024-11-08',
        municipio: 'Vale Real',
        observacao: 'Consulta médica',
      },
      {
        assistido: 'Carlos Souza',
        profissional: 'Dra. Julia Ferreira',
        data: '2024-11-10',
        municipio: 'Feliz',
        observacao: 'Não informado',
      },
    ];
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório de Faltas gerado com sucesso!');
    // Aqui seria implementada a lógica real de geração do relatório
    console.log('Gerar relatório com filtros aplicados:', this.filtrosForm.value);
  }

  profissionaisOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Dr. Maria Santos' },
    { value: '2', label: 'Dr. Pedro Lima' },
    { value: '3', label: 'Dra. Julia Ferreira' },
  ];

  municipiosOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Feliz' },
    { value: '2', label: 'Vale Real' },
    { value: '3', label: 'Linha Nova' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'assistido', label: 'Assistido', width: 'large', align: 'left' },
    { key: 'profissional', label: 'Profissional', width: 'large', align: 'left' },
    { key: 'data', label: 'Data', width: 'medium', align: 'center' },
    { key: 'municipio', label: 'Município', width: 'medium', align: 'left' },
    { key: 'observacao', label: 'Observação', width: 'large', align: 'left' },
  ];
}
