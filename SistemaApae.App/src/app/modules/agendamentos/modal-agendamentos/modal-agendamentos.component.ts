import { Component, OnInit, Inject, inject, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalComponent } from '../../core/base-modal/base-modal.component';
import { ModalData } from '../../core/services/modal.service';
import { InputComponent } from '../../core/input/input.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { map, Observable } from 'rxjs';
import { Agendamento, DiaDaSemana, TipoRecorrencia } from '../agendamento';
import { AgendamentoService } from '../agendamento.service';
import { NotificationService } from '../../core/notification/notification.service';
import { Status } from '../../core/enum/status.enum';
import { AutocompleteMultipleComponent } from '../../core/multi-autocomplete/multi-autocomplete.component';
import { AssistidoService } from '../../assistidos/assistido.service';
import { Usuario } from '../../usuarios/usuario';
import { Roles } from '../../auth/roles.enum';
import { UsuarioService } from '../../usuarios/usuario.service';
import { AutocompleteComponent } from '../../core/autocomplete/autocomplete.component';
import { MatRadioModule } from '@angular/material/radio';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';
import { DateUtils } from '../../core/date/date-utils';

@Component({
  selector: 'app-modal-usuarios',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    SelectComponent,
    InputComponent,
    BaseModalComponent,
    AutocompleteMultipleComponent,
    AutocompleteComponent,
    MatRadioModule,
    DatepickerComponent,
  ],
  templateUrl: './modal-agendamentos.component.html',
  styleUrls: ['./modal-agendamentos.component.less'],
})
export class ModalAgendamentosComponent implements OnInit {
  @ViewChild(`multiAutocomplete`) multiAutocompleteComponent!: AutocompleteMultipleComponent;

  protected formCadastro!: FormGroup;
  private isEdit: boolean = false;

  private agendamentoService: AgendamentoService = inject(AgendamentoService);

  private assistidoService: AssistidoService = inject(AssistidoService);

  private usuarioService: UsuarioService = inject(UsuarioService);

  public tipoRecorrencia = TipoRecorrencia;

  diaDaSemanaOptions: SelectOption[] = [
    { value: DiaDaSemana.SEGUNDA, label: 'Segunda' },
    { value: DiaDaSemana.TERCA, label: 'Terça' },
    { value: DiaDaSemana.QUARTA, label: 'Quarta' },
    { value: DiaDaSemana.QUINTA, label: 'Quinta' },
    { value: DiaDaSemana.SEXTA, label: 'Sexta' },
    { value: DiaDaSemana.SABADO, label: 'Sábado' },
  ];

  statusOptions: SelectOption[] = [
    { value: Status.Ativo, label: 'Ativo' },
    { value: Status.Inativo, label: 'Inativo' },
  ];

  assistidosOptions$: Observable<SelectOption[]> = this.assistidoService.listarAssistidos({}).pipe(
    map((assistidos) =>
      assistidos.map((assistido) => ({
        value: assistido.id,
        label: assistido.nome,
      }))
    )
  );

  profissionalOptions$: Observable<SelectOption[]> = this.buscarProfissionais().pipe(
    map((users) =>
      users.map((user) => ({
        value: user.id,
        label: user.nome,
      }))
    )
  );

  constructor(
    private formBuilder: UntypedFormBuilder,
    public dialogRef: MatDialogRef<ModalAgendamentosComponent>,
    private notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {}

  ngOnInit(): void {
    this.isEdit = !!this.data?.data.isEdit;
    this.initFormCadastro();
  }

  initFormCadastro() {
    const object: Agendamento = this.data.element;

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      profissional: [
        this.isEdit ? { value: object?.profissional.id, label: object.profissional.nome } : null,
        Validators.required,
      ],
      assistidos: [
        this.isEdit ? object.assistidos.map((i) => ({ label: i.nome, value: i.id })) : null,
        [Validators.required],
      ],
      tipoRecorrencia: [object?.tipoRecorrencia || TipoRecorrencia.NENHUM, Validators.required],
      status: [
        { value: object?.status || Status.Ativo, disabled: !this.isEdit },
        Validators.required,
      ],
      horarioAgendamento: [
        object?.horarioAgendamento ? object.horarioAgendamento.toString().slice(0, 5) : '',
        [Validators.required, Validators.pattern(/^([0-1]?[0-9]|2[0-3]):[0-5][0-9]$/)],
      ],
      diaSemana: [
        { value: object?.diaSemana, disabled: object?.tipoRecorrencia != TipoRecorrencia.SEMANAL },
        object?.tipoRecorrencia == TipoRecorrencia.SEMANAL ? Validators.required : [],
      ],
      dataAgendamento: [
        {
          value: object?.dataAgendamento ? DateUtils.fromDbToField(object?.dataAgendamento as any) : '', 
          disabled: object && object?.tipoRecorrencia != TipoRecorrencia.NENHUM,
        },
        object?.tipoRecorrencia == TipoRecorrencia.NENHUM || !object ? Validators.required : [],
      ],
      observacao: [object?.observacao || ''],
    });

    if (this.data.isVisualizacao) {
      this.formCadastro.disable();
    }

    this.formCadastro.get(`tipoRecorrencia`)?.valueChanges.subscribe((value) => {
      this.formCadastro.get(`diaSemana`)?.reset();
      this.formCadastro.get(`dataAgendamento`)?.reset();

      if (value == TipoRecorrencia.NENHUM) {
        this.formCadastro.get(`diaSemana`)?.clearValidators();
        this.formCadastro.get(`diaSemana`)?.disable();

        this.formCadastro.get(`dataAgendamento`)?.addValidators(Validators.required);
        this.formCadastro.get(`dataAgendamento`)?.enable();
      } else {
        this.formCadastro.get(`diaSemana`)?.addValidators(Validators.required);
        this.formCadastro.get(`diaSemana`)?.enable();

        this.formCadastro.get(`dataAgendamento`)?.clearValidators();
        this.formCadastro.get(`dataAgendamento`)?.disable();
      }
    });
  }

  valueFromForm(): Agendamento {
    const valor = this.formCadastro.value;

    if (valor.dataAgendamento)
      valor.dataAgendamento = new Date(valor.dataAgendamento).toISOString().slice(0, 10);

    return {
      ...valor,
      assistidos: (valor.assistidos as SelectOption[]).map((v) => ({
        id: v.value
      })),
      profissional: { id: (valor.profissional as SelectOption).value },
    } as Agendamento;
  }

  onConfirm(): void {
    this.multiAutocompleteComponent.controlInput.markAsDirty();
    this.formCadastro.markAllAsTouched();

    if (!this.formCadastro.valid) {
      this.formCadastro.updateValueAndValidity();

      this.notificationService.showWarning(
        'Campos obrigatórios não preenchidos. Verifique os campos destacados.'
      );
      return;
    }

    this.formCadastro.get('UpdatedAt')?.setValue(new Date(), { emitEvent: false });
    if (this.isEdit) {
      this.agendamentoService.editar(this.valueFromForm()).subscribe((val) => {
        this.notificationService.showSuccess('Agendamento editado com sucesso!');
        this.dialogRef.close(true);
      });
    } else {
      this.agendamentoService.salvar(this.valueFromForm()).subscribe((val) => {
        this.notificationService.showSuccess('Agendamento salvo com sucesso!');
        this.dialogRef.close(true);
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
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
}
