import { Component, inject, Inject, OnInit, ChangeDetectorRef } from '@angular/core';
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
import { Assistido } from '../../assistidos/assistido';
import { AssistidoService } from '../../assistidos/assistido.service';
import { Agendamento } from '../../agendamentos/agendamento';
import { AgendamentoService } from '../../agendamentos/agendamento.service';
import { Observable, map } from 'rxjs';
import { DatepickerComponent } from '../../core/date/datepicker/datepicker.component';
import { UsuarioService } from '../../usuarios/usuario.service';
import { Roles } from '../../auth/roles.enum';
import { Usuario } from '../../usuarios/usuario';

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

  private usuarioService = inject(UsuarioService);

  private assistidoService: AssistidoService = inject(AssistidoService);

  protected assistidosOptions$: any;

  protected profissionalOptions$: any;

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
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.isEdit = data?.isEdit || false;
    this.isVisualizacao = data?.isVisualizacao || false;
    this.atendimento = data?.element;

    this.assistidosOptions$ = this.assistidoService.listarAssistidos({}).pipe(
      map((assistidos) =>
        assistidos.map((assistido) => ({
          value: assistido.id,
          label: assistido.nome,
        }))
      )
    );

    this.profissionalOptions$ = this.buscarProfissionais().pipe(
      map((users) =>
        users.map((user) => ({
          value: user.id,
          label: user.nome,
        }))
      )
    );
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

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.form = this.formBuilder.group({
      profissional: ['', Validators.required],
      assistido: ['', Validators.required],
      dataAtendimento: ['', Validators.required],
      presenca: ['', Validators.required],
      avaliacao: [''],
      observacao: [''],
      status: [Status.Ativo, Validators.required],
    });

    if (this.isVisualizacao) {
      this.form.disable();
    }
    this.preencherForm();
  }

  preencherForm() {
    if (this.atendimento) {
      this.form.get('profissional')?.setValue(this.atendimento!.profissional, { emitEvent: false });

      this.form.get('assistido')?.setValue(this.atendimento!.assistido.id, { emitEvent: false });

      this.form
        .get('dataAtendimento')
        ?.setValue(this.atendimento!.dataAtendimento, { emitEvent: false });
      this.form.get('presenca')?.setValue(this.atendimento!.presenca, { emitEvent: false });
      this.form.get('avaliacao')?.setValue(this.atendimento!.avaliacao, { emitEvent: false });
      this.form.get('observacao')?.setValue(this.atendimento!.observacao, { emitEvent: false });
      this.form.get('status')?.setValue(this.atendimento!.status, { emitEvent: false });
    }
  }

  salvar() {
    if (this.form.valid) {
      const formValue = this.form.value;

      if (this.isEdit && this.atendimento) {
        const atendimentoAtualizado = new Atendimento({
          ...this.atendimento,
          ...formValue,
        });

        this.atendimentoService.editar(atendimentoAtualizado).subscribe(() => {
          this.dialogRef.close(true);
        });
      } else {
        // Criar novo atendimento
        const novoAtendimento = new Atendimento(formValue);

        this.atendimentoService.salvar(novoAtendimento).subscribe(() => {
          this.dialogRef.close(true);
        });
      }
    } else {
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
