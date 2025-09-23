import { Component, OnInit, Inject, ViewEncapsulation } from '@angular/core';
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
import { OrgaoClasse } from '../cadastro-usuario.interface';

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
    { value: 'Coordenador', label: 'Coordenador' },
    { value: 'Profissional', label: 'Profissional' },
  ];

  statusOptions: SelectOption[] = [
    { value: 'ativo', label: 'Ativo' },
    { value: 'inativo', label: 'Inativo' },
  ];

  protected orgaoClasseOptions: SelectOption[] = [
    { value: OrgaoClasse.CRP, label: 'CRP' },
    { value: OrgaoClasse.CREFITO, label: 'CREFITO' },
    { value: OrgaoClasse.CFM, label: 'CFM' },
    { value: OrgaoClasse.CRO, label: 'CRO' },
    { value: OrgaoClasse.CRM, label: 'CRM' },
    { value: OrgaoClasse.COREME, label: 'COREME' },
    { value: OrgaoClasse.CRN, label: 'CRN' },
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

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      email: [object?.email || '', [Validators.required, Validators.email]],
      tipo: [object?.tipo || 'Profissional', Validators.required],
      status: [object?.status || 'ativo', Validators.required],
      especialidade: [object?.especialidade],
      orgaoClasse: [object?.orgaoClasse],
      registroProfissional: [object?.registroProfissional],
    });
  }

  onConfirm(): void {
    if (this.formCadastro.invalid) {
      this.formCadastro.markAllAsTouched();
      return;
    }
    console.log('Formulário enviado:', this.formCadastro.value);
    if (this.formCadastro.valid) {
      this.dialogRef.close(this.formCadastro.value);
    }
  }

  onCancel(): void {
    console.log('Cancel clicked');
    this.dialogRef.close(null);
  }
}
