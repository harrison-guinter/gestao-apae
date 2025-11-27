import { Component, inject, Inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { Atendimento, StatusAtendimentoEnum } from '../atendimento';
import { AtendimentoService } from '../atendimento.service';
import { Status } from '../../core/enum/status.enum';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';
import { AtendimentoPendente } from '../atendimentos-pendentes/atendimento-pendente.interface';
import { DateUtils } from '../../core/date/date-utils';
import { NotificationService } from '../../core/notification/notification.service';

export interface ModalAtendimentosData {
  isEdit: boolean;
}

@Component({
  selector: 'app-modal-atendimentos',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatDialogModule,
    SelectComponent,
    DatepickerComponent,
  ],
  templateUrl: './modal-atendimentos.component.html',
  styleUrls: ['./modal-atendimentos.component.less'],
})
export class ModalAtendimentosComponent {
  form!: UntypedFormGroup;
  isVisualizacao: boolean = false;

  assistidosOptions: SelectOption[] = [];
  profissionalOptions: SelectOption[] = [];

  presencaOptions: SelectOption[] = [
    { value: StatusAtendimentoEnum.PRESENTE, label: 'Presente' },
    { value: StatusAtendimentoEnum.FALTA, label: 'Falta' },
    { value: StatusAtendimentoEnum.FALTA_JUSTIFICADA, label: 'Falta Justificada' },
  ];

  statusOptions: SelectOption[] = [
    { value: Status.Ativo, label: 'Ativo' },
    { value: Status.Inativo, label: 'Inativo' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private atendimentoService: AtendimentoService,
    private dialogRef: MatDialogRef<ModalAtendimentosComponent>,
    private cdr: ChangeDetectorRef,
    private notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isVisualizacao = data?.isVisualizacao || false;

    if (this.isVisualizacao) {
      this.initFormRealizado(data.element);
    } else {
      this.initFormPendente(data.element);
    }

    this.profissionalOptions = [
      { value: data.element.profissional.id, label: data.element.profissional.nome },
    ];
    this.assistidosOptions = [
      { value: data.element.assistido.id, label: data.element.assistido.nome },
    ];
  }

  initFormPendente(atendimentoPendente: AtendimentoPendente) {

    this.form = this.formBuilder.group({
      profissional: [
        {
          value: {
            value: atendimentoPendente?.profissional.id,
            label: atendimentoPendente.profissional.nome,
          },
          disabled: true,
        },
        Validators.required,
      ],
      assistido: [
        {
          value: {
            value: atendimentoPendente?.assistido.id,
            label: atendimentoPendente.assistido.nome,
          },
          disabled: true,
        },
        Validators.required,
      ],
      dataAtendimento: [{value: DateUtils.fromDbToField(atendimentoPendente.dataAgendamento as any), disabled: true }, Validators.required],
      presenca: ['', Validators.required],
      avaliacao: [''],
      observacao: [''],
      status: [{ value: Status.Ativo, disabled: true }, Validators.required],
      agendamento: [{ value: atendimentoPendente.agendamento, disabled: true }],
    });

    if (this.isVisualizacao) {
      this.form.disable();
    }
  }

  initFormRealizado(atendimento: Atendimento) {
    this.form = this.formBuilder.group({
      profissional: [
        { value: atendimento?.profissional.id, label: atendimento?.profissional.nome },
      ],
      assistido: [{ value: atendimento?.assistido.id, label: atendimento?.assistido.nome }],
      dataAtendimento: [atendimento?.dataAtendimento],
      presenca: [atendimento.presenca],
      avaliacao: [atendimento.avaliacao],
      observacao: [atendimento.observacao],
      status: [atendimento.status],
      agendamento: [atendimento.agendamento],
    });

    if (this.isVisualizacao) {
      this.form.disable();
    }
  }

  salvar() {
    if (this.form.valid) {
      const formValue = this.form.getRawValue();

      formValue.profissional = { id: formValue.profissional.value, nome: formValue.profissional.label };
      formValue.assistido = { id: formValue.assistido.value, nome: formValue.assistido.label };

      const novoAtendimento = new Atendimento(formValue);
      novoAtendimento.dataAtendimento = new Date();

      this.atendimentoService.salvar(novoAtendimento).subscribe({
        next: (response) => {
          this.notificationService.showSuccess('Atendimento realizado com sucesso!');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Erro ao criar atendimento:', error);
        },
      });
    } else {
      this.form.markAllAsTouched();
    }
  }

  cancelar() {
    this.dialogRef.close(false);
  }

  get titulo(): string {
    if (this.isVisualizacao) {
      return 'Visualizar Atendimento';
    }
    return 'Realizar Atendimento';
  }
}
