import { Component, inject, OnInit } from '@angular/core';
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
import { PageInfoService } from '../../core/services/page-info.service';
import { FiltersContainerComponent } from '../../core/filters-container/filters-container.component';
import {
  RelatorioIndividualFiltro,
  RelatorioIndividualService,
} from './relatorio-individual.service';
import { AssistidoService } from '../../assistidos/assistido.service';
import { UsuarioService } from '../../usuarios/usuario.service';
import { map, Observable } from 'rxjs';
import { Roles } from '../../auth/roles.enum';
import { Status } from '../../core/enum/status.enum';
import { Usuario } from '../../usuarios/usuario';
import { CidadesService } from '../../cidades/cidades.service';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';
import { AutocompleteComponent } from '../../core/autocomplete/autocomplete.component';

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
    FiltersContainerComponent,
    DatepickerComponent,
    AutocompleteComponent,
  ],
  templateUrl: './relatorio-individual.component.html',
  styleUrls: ['./relatorio-individual.component.less'],
})
export class RelatorioIndividualComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: any[] = [];

  private relatorioIndividualService = inject(RelatorioIndividualService);
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

  constructor(private formBuilder: UntypedFormBuilder, private pageInfoService: PageInfoService) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório Individual',
      'Relatório individualizado por assistido'
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
    this.relatorioIndividualService
      .listarAtendimentos(this.valueFromForm(this.filtrosForm.value))
      .subscribe((data) => {
        this.relatorioData = data.map((item) => ({
          data: new Date(item.atendimento.data).toLocaleDateString('pt-BR'),
          profissional: item.profissional.nome,
          municipio: item.municipio.nome,
          assistido: item.assistido.nome,
        }));
      });
  }

  valueFromForm(formValue: any): RelatorioIndividualFiltro {
    console.log(formValue);
    return {
      dataInicio: formValue?.dataInicio
        ? new Date(formValue?.dataInicio).toISOString().split('T')[0]
        : undefined,
      dataFim: formValue?.dataFim
        ? new Date(formValue?.dataFim).toISOString().split('T')[0]
        : undefined,
      idAssistido: formValue.assistido?.value?.id,
      idProfissional: formValue.profissional?.value?.id,
      idMunicipio: formValue?.municipio,
    };
  }

  tableColumns: TableColumn[] = [
    { key: 'data', label: 'Data', width: 'medium', align: 'left' },
    { key: 'profissional', label: 'Profissional', width: 'large', align: 'left' },
    { key: 'assistido', label: 'Assistido', width: 'large', align: 'left' },
    { key: 'municipio', label: 'Município', width: 'large', align: 'left' },
  ];
}
