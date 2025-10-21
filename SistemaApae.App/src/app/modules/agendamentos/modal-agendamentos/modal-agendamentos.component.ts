import { Component, OnInit, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalComponent } from '../../core/base-modal/base-modal.component';
import { ModalData } from '../../core/services/modal.service';
import { InputComponent } from '../../core/input/input.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { CidadesService } from '../../cidades/cidades.service';
import { map, Observable } from 'rxjs';
import { Agendamento } from '../agendamento';
import { AgendamentoService } from '../agendamento.service';
import { NotificationService } from '../../core/notification/notification.service';
import { Status } from '../../core/enum/status.enum';
import { AutocompleteMultipleComponent } from '../../core/multi-autocomplete/multi-autocomplete.component';
import { Assistido } from '../../assistidos/assistido';
import { AssistidoService } from '../../assistidos/assistido.service';
import { Usuario } from '../../usuarios/usuario';
import { Roles } from '../../auth/roles.enum';
import { UsuarioService } from '../../usuarios/usuario.service';
import { AutocompleteComponent } from '../../core/autocomplete/autocomplete.component';

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
    AutocompleteComponent
  ],
  templateUrl: './modal-agendamentos.component.html',
  styleUrls: ['./modal-agendamentos.component.less'],
})
export class ModalAgendamentosComponent implements OnInit {
  protected formCadastro!: FormGroup;
  private isEdit: boolean = false;

  private agendamentoService: AgendamentoService = inject(AgendamentoService);

  private assistidoService: AssistidoService = inject(AssistidoService);

  private usuarioService: UsuarioService = inject(UsuarioService);

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
        value: user, // objeto completo
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
      profissional: [object?.profissional || '', Validators.required],
      assistidos: [object?.assistidos || '', Validators.required],
      status: [object?.status, Validators.required],
      observacao: [object?.observacao || ''],
    });

    if (this.data.isVisualizacao) {
      this.formCadastro.disable();
    }
  }

  valueFromForm(): Agendamento {
    const valor = this.formCadastro.value;

    return { ...valor } as Agendamento;
  }

  onConfirm(): void {
    if (!this.formCadastro.valid) {
      this.formCadastro.markAllAsTouched();
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
        this.dialogRef.close();
      });
    } else {
      this.agendamentoService.salvar(this.valueFromForm()).subscribe((val) => {
        this.notificationService.showSuccess('Agendamento salvo com sucesso!');
        this.dialogRef.close();
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

    private buscarProfissionais(): Observable<Usuario[]> {
      return this.usuarioService.filtrarUsuarios({perfil: Roles.PROFISSIONAL, status: Status.Ativo}).pipe(
        map((users) => {
          return users
            .map((u) => new Usuario(u))
        })
      );
    }
  
}
