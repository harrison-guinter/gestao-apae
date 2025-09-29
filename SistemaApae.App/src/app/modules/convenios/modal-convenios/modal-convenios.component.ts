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
import { Convenio } from '../convenio';

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
    map(cidades => cidades.map(cidade => ({ value: cidade.Id, label: cidade.Nome })))
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
    const object: Convenio = this.data.element;

    this.formCadastro = this.formBuilder.group({
      Id: [object?.Id || null],
      Nome: [object?.Nome || '', Validators.required],
      Status: [object?.Status, Validators.required],
      Municipio: [object?.Municipio ? [{value: object.Municipio.Id, label: object.Municipio.Nome}] : null, Validators.required],
      Observacao: [object?.Observacao || ''],
    });

    if (this.data.isVisualizacao) {
      this.formCadastro.disable();
    }
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
