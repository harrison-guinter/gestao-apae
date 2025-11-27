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
import { PresencaFiltro, RelatorioPresencasService } from './relatorio-presenca.service';
import { AssistidoService } from '../../assistidos/assistido.service';
import { UsuarioService } from '../../usuarios/usuario.service';
import { map } from 'rxjs/internal/operators/map';
import { Observable } from 'rxjs';
import { Roles } from '../../auth/roles.enum';
import { Status } from '../../core/enum/status.enum';
import { Usuario } from '../../usuarios/usuario';
import { RelatorioPresenca } from './relatorio-presenca.interface';
import { CidadesService } from '../../cidades/cidades.service';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';
import { ConvenioService } from '../../convenios/convenio.service';

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
    DatepickerComponent,
  ],
  templateUrl: './relatorio-presenca.component.html',
  styleUrls: ['./relatorio-presenca.component.less'],
})
export class RelatorioPresencaComponent implements OnInit {
  protected filtrosForm!: UntypedFormGroup;
  relatorioData: RelatorioPresenca[] = [];

  private relatorioPresencaService = inject(RelatorioPresencasService);
  private assistidoService = inject(AssistidoService);
  private usuarioService = inject(UsuarioService);
  private municipioService = inject(CidadesService);
  private convenioService: ConvenioService = inject(ConvenioService);

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

  convenios$ = this.convenioService
    .listarConvenios({} as any)
    .pipe(
      map((convenios) =>
        convenios.map((convenio) => ({ value: convenio.id, label: convenio.nome }))
      )
    );

  constructor(
    private formBuilder: UntypedFormBuilder,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService
  ) {}

  ngOnInit() {
    this.pageInfoService.updatePageInfo(
      'Relatório de Presenças',
      'Registro de presenças por assistido'
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
      convenio: [''],
    });
    this.pesquisar();
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.pesquisar();
  }

  pesquisar() {
   
    this.relatorioPresencaService
      .listarPresenca(this.valueFromForm(this.filtrosForm.value))
      .subscribe((data) => {
        console.log(data);
        this.relatorioData = (data as any).itens;
      });
  }

  valueFromForm(formValue: any): PresencaFiltro {
   
    return {
      dataInicio: formValue.dataInicio?.toISOString().split('T')[0],
      dataFim: formValue.dataFim?.toISOString().split('T')[0],
      idAssistido: formValue.assistido?.id,
      idProfissional: formValue.profissional?.id,
      idMunicipio: formValue.municipio?.id,
      idConvenio: formValue.convenio,
    };
  }

  gerarRelatorio() {
    this.notificationService.showSuccess('Relatório de Presença gerado com sucesso!');
  }

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Assistido', width: 'large', align: 'left' },
    { key: 'dataNascimento', label: 'Data nascimento', width: 'medium', align: 'center', type: 'datetime' },
    { key: 'tipoAtendimento', label: 'Tipo de atendimento', width: 'medium', align: 'left' },
    { key: 'endereco', label: 'Endereço', width: 'medium', align: 'left' },
    { key: 'diaTerapias', label: 'D. Terapias', width: 'large', align: 'left' },
    { key: 'diaSemana', label: 'Dia da semana', width: 'medium', align: 'center' },
    { key: 'turno', label: 'Turno', width: 'medium', align: 'center' },
  ];
}
