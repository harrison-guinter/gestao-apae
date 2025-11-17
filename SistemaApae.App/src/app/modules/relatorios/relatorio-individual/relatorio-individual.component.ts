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

interface RelatorioIndividual {
  data: string;
  profissional: string;
  municipio: string;
  tipoAtendimento: string;
  observacao: string;
}

@Component({
  selector: 'app-relatorio-individual',
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
  templateUrl: './relatorio-individual.component.html',
  styleUrls: ['./relatorio-individual.component.less'],
})
export class RelatorioIndividualComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioIndividual[] = [];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório Individual',
      'Relatório individualizado por assistido'
    );
    this.initFiltrosForm();
  }

  initFiltrosForm() {
    this.filtrosForm = this.formBuilder.group({
      assistido: [''],
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
        data: '2024-10-15',
        profissional: 'Dr. Maria Santos',
        municipio: 'Feliz',
        tipoAtendimento: 'Fisioterapia',
        observacao: 'Assistido apresentou melhora significativa',
      },
      {
        data: '2024-10-22',
        profissional: 'Dra. Julia Ferreira',
        municipio: 'Feliz',
        tipoAtendimento: 'Psicologia',
        observacao: 'Sessão de acompanhamento psicológico',
      },
      {
        data: '2024-11-05',
        profissional: 'Dr. Pedro Lima',
        municipio: 'Vale Real',
        tipoAtendimento: 'Fonoaudiologia',
        observacao: 'Exercícios de fala aplicados',
      },
      {
        data: '2024-11-12',
        profissional: 'Dr. Maria Santos',
        municipio: 'Feliz',
        tipoAtendimento: 'Fisioterapia',
        observacao: 'Continuidade do tratamento',
      },
    ];
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório Individual gerado com sucesso!');
    // Aqui seria implementada a lógica real de geração do relatório
    console.log('Filtros aplicados:', this.filtrosForm.value);
  }

  municipiosOptions: SelectOption[] = [
    { value: '', label: 'Todos' },
    { value: '1', label: 'Feliz' },
    { value: '2', label: 'Vale Real' },
    { value: '3', label: 'Linha Nova' },
  ];

  tableColumns: TableColumn[] = [
    { key: 'data', label: 'Data', width: 'medium', align: 'center' },
    { key: 'profissional', label: 'Profissional', width: 'large', align: 'left' },
    { key: 'municipio', label: 'Município/Prefeitura/CAS', width: 'large', align: 'left' },
    { key: 'tipoAtendimento', label: 'Tipo de Atendimento', width: 'medium', align: 'left' },
    { key: 'observacao', label: 'Observação', width: 'xlarge', align: 'left' },
  ];
}
