import { Component, Inject, OnInit } from '@angular/core';
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
import { DatepickerComponent } from '../../core/datepicker/datepicker.component';
import { Atendimento, StatusAtendimentoEnum } from '../atendimento';
import { AtendimentoService } from '../atendimento.service';
import { Status } from '../../core/enum/status.enum';
import { Assistido } from '../../assistidos/assistido';
import { AssistidoService } from '../../assistidos/assistido.service';
import { Agendamento } from '../../agendamentos/agendamento';
import { AgendamentoService } from '../../agendamentos/agendamento.service';
import { Observable, map } from 'rxjs';

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
export class ModalAtendimentosComponent implements OnInit {
  form!: UntypedFormGroup;
  isEdit: boolean = false;
  isVisualizacao: boolean = false;
  atendimento?: Atendimento;

  assistidoOptions: Observable<SelectOption[]>;
  agendamentoOptions: Observable<SelectOption[]>;

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
    private assistidoService: AssistidoService,
    private agendamentoService: AgendamentoService,
    private dialogRef: MatDialogRef<ModalAtendimentosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isEdit = data?.isEdit || false;
    this.isVisualizacao = data?.isVisualizacao || false;
    this.atendimento = data?.element;

    this.assistidoOptions = this.assistidoService.listarAssistidos({}).pipe(
      map((assistidos) =>
        assistidos.map((assistido) => ({
          value: assistido.id!,
          label: assistido.nome!,
        }))
      )
    );

    this.agendamentoOptions = this.agendamentoService.listarAgendamentos({}).pipe(
      map((agendamentos) =>
        agendamentos.map((agendamento) => ({
          value: agendamento.id,
          label: `${agendamento.nome} - ${agendamento.data.toLocaleDateString()} ${
            agendamento.hora
          }`,
        }))
      )
    );
  }

  ngOnInit() {
    this.initForm();
    if (this.atendimento) {
      this.preencherForm();
    }
  }

  initForm() {
    this.form = this.formBuilder.group({
      idAgendamento: ['', Validators.required],
      idAssistido: ['', Validators.required],
      dataAtendimento: ['', Validators.required],
      presenca: ['', Validators.required],
      avaliacao: [''],
      observacao: [''],
      status: [Status.Ativo, Validators.required],
    });

    if (this.isVisualizacao) {
      this.form.disable();
    }
  }

  preencherForm() {
    if (this.atendimento) {
      this.form.patchValue({
        idAgendamento: this.atendimento.idAgendamento,
        idAssistido: this.atendimento.idAssistido,
        dataAtendimento: this.atendimento.dataAtendimento,
        presenca: this.atendimento.presenca,
        avaliacao: this.atendimento.avaliacao,
        observacao: this.atendimento.observacao,
        status: this.atendimento.status,
      });
    }
  }

  salvar() {
    if (this.form.valid) {
      const formValue = this.form.value;

      if (this.isEdit && this.atendimento) {
        // Editar atendimento existente
        const atendimentoAtualizado = new Atendimento({
          ...this.atendimento,
          ...formValue,
        });

        this.atendimentoService.editar(atendimentoAtualizado).subscribe({
          next: (response) => {
            console.log('Atendimento atualizado:', response);
            this.dialogRef.close(true);
          },
          error: (error) => {
            console.error('Erro ao atualizar atendimento:', error);
          },
        });
      } else {
        // Criar novo atendimento
        const novoAtendimento = new Atendimento(formValue);

        this.atendimentoService.salvar(novoAtendimento).subscribe({
          next: (response) => {
            console.log('Atendimento criado:', response);
            this.dialogRef.close(true);
          },
          error: (error) => {
            console.error('Erro ao criar atendimento:', error);
          },
        });
      }
    } else {
      console.log('Formulário inválido');
      this.markFormGroupTouched();
    }
  }

  cancelar() {
    this.dialogRef.close(false);
  }

  get titulo(): string {
    if (this.isVisualizacao) {
      return 'Visualizar Atendimento';
    }
    return this.isEdit ? 'Editar Atendimento' : 'Novo Atendimento';
  }

  private markFormGroupTouched() {
    Object.keys(this.form.controls).forEach((key) => {
      const control = this.form.get(key);
      control?.markAsTouched();
    });
  }
}
