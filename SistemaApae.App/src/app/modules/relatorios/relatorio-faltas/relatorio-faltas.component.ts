import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
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
import { FaltasFiltro, RelatorioFaltasService } from './relatorio-faltas.service';
import { AssistidoService } from '../../assistidos/assistido.service';
import { UsuarioService } from '../../usuarios/usuario.service';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs';
import { Roles } from '../../auth/roles.enum';
import { Status } from '../../core/enum/status.enum';
import { Usuario } from '../../usuarios/usuario';
import { RelatorioFaltas } from './relatorio-faltas.interface';
import { CidadesService } from '../../cidades/cidades.service';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';

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
    DatepickerComponent,
  ],
  templateUrl: './relatorio-faltas.component.html',
  styleUrls: ['./relatorio-faltas.component.less'],
})
export class RelatorioFaltasComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioFaltas[] = [];

  private relatorioFaltasService = inject(RelatorioFaltasService);
  private assistidoService = inject(AssistidoService);
  private usuarioService = inject(UsuarioService);
  private municipioService = inject(CidadesService);

  profissionalOptions$: Observable<SelectOption[]> = this.buscarProfissionais().pipe(
    map((users) =>
      users.map((user) => ({
        value: user,
        label: user.nome,
      }))
    )
  );

  assistidosOptions$: Observable<SelectOption[]> = this.assistidoService.listarAssistidos({}).pipe(
    map((assistidos) =>
      assistidos.map((assistido) => ({
        value: assistido,
        label: assistido.nome,
      }))
    )
  );

  cidades$ = this.municipioService
    .listarCidades()
    .pipe(map((cidades) => cidades.map((cidade) => ({ value: cidade.id, label: cidade.nome }))));

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

  private buscarProfissionais(): Observable<Usuario[]> {
    return this.usuarioService
      .filtrarUsuarios({ perfil: Roles.PROFISSIONAL, status: Status.Ativo })
      .pipe(
        map((users) => {
          return users.map((u) => new Usuario(u));
        })
      );
  }

  initFiltrosForm() {
    const hoje = new Date();
    const primeiroDia: Date = new Date(hoje.getFullYear(), hoje.getMonth(), 1);
    const ultimoDia: Date = new Date(hoje.getFullYear(), hoje.getMonth() + 1, 0);

    this.filtrosForm = this.formBuilder.group({
      profissional: [''],
      assistido: [''],
      dataInicio: [primeiroDia],
      dataFim: [ultimoDia],
      municipio: [''],
    });
    this.pesquisar();
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.pesquisar();
  }

  pesquisar() {
    this.relatorioFaltasService
      .listarFaltas(this.valueFromForm(this.filtrosForm.value))
      .subscribe((data) => {
        this.relatorioData = data;
      });
  }

  valueFromForm(formValue: any): FaltasFiltro {
    console.log(typeof formValue.dataInicio);
    return {
      dataInicio: formValue.dataInicio.toISOString().split('T')[0],
      dataFim: formValue.dataFim.toISOString().split('T')[0],
      idAssistido: formValue.assistido.id,
      idProfissional: formValue.profissional.id,
      idMunicipio: formValue.municipio.id,
    };
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório de Faltas gerado com sucesso!');
    // Aqui seria implementada a lógica real de geração do relatório
    console.log('Gerar relatório com filtros aplicados:', this.filtrosForm.value);
  }

  tableColumns: TableColumn[] = [
    { key: 'nomeAssistido', label: 'Assistido', width: 'large', align: 'left' },
    { key: 'nomeProfissional', label: 'Profissional', width: 'large', align: 'left' },
    { key: 'dataAtendimento', label: 'Data', width: 'medium', align: 'center', type: 'datetime' },
    { key: 'nomeMunicipio', label: 'Município', width: 'medium', align: 'left' },
    { key: 'nomeConvenio', label: 'Convênio', width: 'medium', align: 'left' },
    { key: 'observacaoAtendimento', label: 'Observação', width: 'large', align: 'left' },
  ];
}
