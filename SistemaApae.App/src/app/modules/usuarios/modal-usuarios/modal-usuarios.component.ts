import { Component, OnInit, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  UntypedFormBuilder,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BaseModalComponent } from '../../core/base-modal/base-modal.component';
import { ModalData } from '../../core/services/modal.service';
import { InputComponent } from '../../core/input/input.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';

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
  ],
  templateUrl: './modal-usuarios.component.html',
  styleUrls: ['./modal-usuarios.component.less'],
})
export class ModalUsuariosComponent extends BaseModalComponent implements OnInit {
  protected formCadastro!: FormGroup;

  protected tiposUsuario: SelectOption[] = [
    { value: 'Coordenador', label: 'Coordenador' },
    { value: 'Profissional', label: 'Profissional' },
  ];

  statusOptions: SelectOption[] = [
    { value: 'ativo', label: 'Ativo' },
    { value: 'inativo', label: 'Inativo' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    public override dialogRef: MatDialogRef<BaseModalComponent>,
    @Inject(MAT_DIALOG_DATA) public override data: ModalData
  ) {
    super(dialogRef, data);
    console.log('Dados recebidos no modal:', data);
  }

  override initFormCadastro() {
    const object = this.data.element;

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      email: [object?.email || '', [Validators.required, Validators.email]],
      tipo: [object?.tipo || 'Profissional', Validators.required],
      status: [object?.status || 'ativo', Validators.required],
      especialidade: [object?.especialidade || ''],
    });
  }

  override onConfirm(): void {
    //Chamar aqui o endpoint de salvar
    if (this.formCadastro.valid) {
      this.dialogRef.close(this.formCadastro.value);
    }
  }

  override isFormInvalid(): boolean {
    return this.formCadastro ? this.formCadastro.invalid : false;
  }
}
