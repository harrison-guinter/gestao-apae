import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, Validators, UntypedFormBuilder } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalComponent } from '../../core/base-modal/base-modal.component';
import { ModalData } from '../../core/services/modal.service';
import { InputComponent } from '../../core/input/input.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { Roles } from '../../auth/roles.enum';
import { StatusUsuarioEnum } from '../usuario';
import { NotificationService } from '../../core/notification/notification.service';
import { UsuarioService } from '../usuario.service';

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
  ],
  templateUrl: './modal-usuarios.component.html',
  styleUrls: ['./modal-usuarios.component.less'],
})
export class ModalUsuariosComponent implements OnInit {
  protected formCadastro!: FormGroup;
  private isEdit: boolean = false;

  protected tiposUsuario: SelectOption[] = [
    { value: Roles.PROFISSIONAL, label: 'Profissional' },
    { value: Roles.COORDENADOR, label: 'Coordenador' },
  ];

  statusOptions: SelectOption[] = [
    { value: StatusUsuarioEnum.ATIVO, label: 'Ativo' },
    { value: StatusUsuarioEnum.INATIVO, label: 'Inativo' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    public dialogRef: MatDialogRef<ModalUsuariosComponent>,
    private notificationService: NotificationService,
    private usuarioService: UsuarioService,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {}

  ngOnInit(): void {
    this.initFormCadastro();
    this.isEdit = !!this.data?.data.isEdit;
    this.setupConditionalValidation();
  }

  initFormCadastro() {
    const object = this.data.element;

    const statusValue = object?.status || StatusUsuarioEnum.ATIVO;
    const perfilValue = object?.perfil || Roles.PROFISSIONAL;

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      email: [object?.email || '', [Validators.required, Validators.email]],
      perfil: [perfilValue, Validators.required],
      especialidade: [object?.especialidade],
      status: [statusValue, Validators.required],
      observacao: [object?.observacao],
      telefone: [object?.telefone],
      registroProfissional: [object?.registroProfissional],
      UpdatedAt: [{ value: object?.UpdatedAt || null, disabled: true }],
    });
  }

  onConfirm(): void {
    if (this.formCadastro.invalid) {
      this.formCadastro.markAllAsTouched();
      this.formCadastro.updateValueAndValidity();

      this.notificationService.showWarning(
        'Campos obrigatórios não preenchidos. Verifique os campos destacados.'
      );
      return;
    }

    this.formCadastro.get('UpdatedAt')?.setValue(new Date(), { emitEvent: false });
    if (this.isEdit) {
      this.usuarioService.editarUsuario(this.formCadastro.value).subscribe((val) => {
        this.notificationService.showSuccess('Usuário editado com sucesso!');
        this.dialogRef.close(this.formCadastro.value);
      });
    } else {
      this.usuarioService.salvarUsuario(this.formCadastro.value).subscribe((val) => {
        this.notificationService.showSuccess('Usuário salvo com sucesso!');
        this.dialogRef.close(this.formCadastro.value);
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close(null);
  }

  private setupConditionalValidation(): void {
    // Configurar validação condicional inicialmente
    this.updateConditionalValidation();

    // Escutar mudanças no campo perfil
    this.formCadastro.get('perfil')?.valueChanges.subscribe(() => {
      this.updateConditionalValidation();
    });
  }

  private updateConditionalValidation(): void {
    const perfilControl = this.formCadastro.get('perfil');
    const especialidadeControl = this.formCadastro.get('especialidade');
    const registroProfissionalControl = this.formCadastro.get('registroProfissional');

    if (perfilControl?.value === Roles.PROFISSIONAL) {
      // Tornar campos obrigatórios para profissionais
      especialidadeControl?.setValidators([Validators.required]);
      registroProfissionalControl?.setValidators([Validators.required]);
    } else {
      // Remover validação obrigatória para coordenadores
      especialidadeControl?.clearValidators();
      registroProfissionalControl?.clearValidators();
    }

    // Atualizar a validação dos campos
    especialidadeControl?.updateValueAndValidity();
    registroProfissionalControl?.updateValueAndValidity();
  }
}
