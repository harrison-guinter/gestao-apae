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
import { MatDatepickerModule } from '@angular/material/datepicker';
import {
  DateAdapter,
  MatNativeDateModule,
  MAT_DATE_FORMATS,
  NativeDateAdapter,
} from '@angular/material/core';
import { AutocompleteComponent } from '../../core/autocomplete/autocomplete.component';

export class MonthYearDateAdapter extends NativeDateAdapter {
  override format(date: Date, displayFormat: Object): string {
    if (displayFormat === 'input') {
      const month = date.getMonth() + 1;
      const year = date.getFullYear();
      return `${month.toString().padStart(2, '0')}/${year}`;
    }
    return date.toDateString();
  }
}

export const MONTH_YEAR_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'input',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

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
    MatDatepickerModule,
    AutocompleteComponent,
  ],
  providers: [
    {
      provide: DateAdapter,
      useClass: MonthYearDateAdapter,
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: MONTH_YEAR_FORMATS,
    },
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
    this.filtrosForm = this.formBuilder.group({
      profissional: [''],
      periodoReferencia: [new Date()],
      municipio: [''],
      convenio: [''],
    });
    this.pesquisar();
  }

  limparFiltros() {
    this.filtrosForm.reset();
    this.filtrosForm.get('periodoReferencia')?.setValue(new Date());
    this.pesquisar();
  }

  pesquisar() {
    this.relatorioPresencaService.listarPresenca(this.valueFromForm()).subscribe((data) => {
      this.relatorioData = (data as any).itens;
    });
  }

  valueFromForm(): PresencaFiltro {
    const formValue = this.filtrosForm.getRawValue();

    const primeiroDia: Date = new Date(
      formValue.periodoReferencia.getFullYear(),
      formValue.periodoReferencia.getMonth(),
      1
    );
    const ultimoDia: Date = new Date(
      formValue.periodoReferencia.getFullYear(),
      formValue.periodoReferencia.getMonth() + 1,
      0
    );

    return {
      dataInicio: primeiroDia?.toISOString().split('T')[0],
      dataFim: ultimoDia?.toISOString().split('T')[0],
      idProfissional: formValue.profissional?.value?.id,
      idMunicipio: formValue.municipio?.value,
      idConvenio: formValue.convenio?.value,
    };
  }

  gerarRelatorio() {
    this.relatorioPresencaService.gerarRelatorio(this.valueFromForm()).subscribe((blob) => {
      const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;

      a.download = 'relatorio_presencas_' + new Date().toISOString() + '.xlsx';

      a.click();
      window.URL.revokeObjectURL(url);
    });
    this.notificationService.showSuccess('Relatório de Presença gerado com sucesso!');
  }

  tableColumns: TableColumn[] = [
    { key: 'nome', label: 'Assistido', width: 'large', align: 'left' },
    {
      key: 'dataNascimento',
      label: 'Data nascimento',
      width: 'medium',
      align: 'center',
      type: 'date',
    },
    { key: 'tipoAtendimento', label: 'Tipo de atendimento', width: 'medium', align: 'left' },
    { key: 'endereco', label: 'Endereço', width: 'medium', align: 'left' },
    { key: 'diaTerapias', label: 'D. Terapias', width: 'large', align: 'left' },
    { key: 'diaSemana', label: 'Dia da semana', width: 'medium', align: 'center' },
    { key: 'turno', label: 'Turno', width: 'medium', align: 'center' },
  ];

  chosenYearHandler(normalizedYear: Date) {
    const ctrlValue = this.filtrosForm.get('periodoReferencia')?.value || new Date();
    ctrlValue.setFullYear(normalizedYear.getFullYear());
    this.filtrosForm.get('periodoReferencia')?.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Date, datepicker: any) {
    const ctrlValue = this.filtrosForm.get('periodoReferencia')?.value || new Date();
    ctrlValue.setMonth(normalizedMonth.getMonth());
    ctrlValue.setFullYear(normalizedMonth.getFullYear());
    this.filtrosForm.get('periodoReferencia')?.setValue(ctrlValue);
    datepicker.close();
  }
}
