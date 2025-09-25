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

  protected tiposUsuario: SelectOption[] = [
    { value: Roles.COORDENADOR, label: 'Coordenador' },
    { value: Roles.PROFISSIONAL, label: 'Profissional' },
  ];

  statusOptions: SelectOption[] = [
    { value: true, label: 'Ativo' },
    { value: false, label: 'Inativo' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    public dialogRef: MatDialogRef<ModalUsuariosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {
    console.log('Dados recebidos no modal:', data);
  }

  ngOnInit(): void {
    this.initFormCadastro();
  }

  initFormCadastro() {
    const object = this.data.element;
    console.log('Elemento recebido no modal:', object);

    const statusDefault = typeof object?.ativo === 'boolean' ? object.ativo : true;

    this.formCadastro = this.formBuilder.group({
      idUsuario: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      email: [object?.email || '', [Validators.required, Validators.email]],
      perfil: [object?.tipo || 2, Validators.required],
      especialidade: [object?.especialidade],
      status: [statusDefault, Validators.required],
      observacao: [object?.observacao],
      telefone: [object?.telefone],
      registroProfissional: [object?.registroProfissional],
    });
  }

  onConfirm(): void {
    if (this.formCadastro.invalid) {
      this.formCadastro.markAllAsTouched();
      return;
    }

    if (this.formCadastro.valid) {
      this.dialogRef.close(this.formCadastro.value);
    }
  }

  onCancel(): void {
    console.log('Cancel clicked');
    this.dialogRef.close(null);
  }
}
