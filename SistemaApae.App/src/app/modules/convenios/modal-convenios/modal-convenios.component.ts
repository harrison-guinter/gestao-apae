import { Component, OnInit, Inject, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
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
import { CidadesService } from '../../cidades/cidades.service';
import { map } from 'rxjs';

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
  templateUrl: './modal-convenios.component.html',
  styleUrls: ['./modal-convenios.component.less'],
})
export class ModalConveniosComponent implements OnInit {
  protected formCadastro!: FormGroup;

  private cidadesService: CidadesService = inject(CidadesService);

  statusOptions: SelectOption[] = [
    { value: true, label: 'Ativo' },
    { value: false, label: 'Inativo' },
  ];

  cidades$ = this.cidadesService.listarCidades().pipe(
    map(cidades => cidades.map(cidade => ({ value: cidade.idMunicipio, label: cidade.nome })))
  );

  constructor(
    private formBuilder: UntypedFormBuilder,
    public dialogRef: MatDialogRef<ModalConveniosComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {

  }

  ngOnInit(): void {
    this.initFormCadastro();
  }

  initFormCadastro() {
    const object = this.data.element;

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      status: [object?.status, Validators.required],
      cidade: [object?.cidade || '', Validators.required],
      observacoes: [object?.observacoes || ''],
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
    this.dialogRef.close();
  }
}
