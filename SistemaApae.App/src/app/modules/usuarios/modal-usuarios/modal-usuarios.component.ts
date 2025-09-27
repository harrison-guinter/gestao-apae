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
    });
  }

  onConfirm(): void {
    if (this.formCadastro.invalid) {
      this.formCadastro.markAllAsTouched();
      this.formCadastro.updateValueAndValidity();
      this.notificationService.showInfo(
        'Campos obrigatórios não preenchidos. Verifique os campos destacados.'
      );
      return;
    }

    if (this.isEdit) {
      this.usuarioService.editarUsuario(this.formCadastro.value).subscribe((val) => {
        this.dialogRef.close(this.formCadastro.value);
      });
    } else {
      this.usuarioService.salvarUsuario(this.formCadastro.value).subscribe((val) => {
        this.dialogRef.close(this.formCadastro.value);
      });
    }
  }

  onCancel(): void {
    console.log('Cancel clicked');
    this.dialogRef.close(null);
  }
}
