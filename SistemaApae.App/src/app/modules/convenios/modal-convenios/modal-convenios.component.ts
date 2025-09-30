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
import { map } from 'rxjs';
import { Convenio } from '../convenio';
import { ConvenioService } from '../convenio.service';
import { NotificationService } from '../../core/notification/notification.service';
import { T } from '@angular/cdk/keycodes';

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
  private isEdit: boolean = false;

  private cidadesService: CidadesService = inject(CidadesService);
  private convenioService: ConvenioService = inject(ConvenioService);

  statusOptions: SelectOption[] = [
    { value: 1, label: 'Ativo' },
    { value: 2, label: 'Inativo' },
  ];

  tipos: SelectOption[] = [
    { value: 1, label: 'CAS' },
    { value: 2, label: 'Educação' },
    { value: 3, label: 'Saúde' },
    { value: 4, label: 'Assistência social' },
    { value: 5, label: 'EJA' },
  ];

  cidades$ = this.cidadesService
    .listarCidades()
    .pipe(map((cidades) => cidades.map((cidade) => ({ value: cidade.id, label: cidade.nome }))));

  constructor(
    private formBuilder: UntypedFormBuilder,
    public dialogRef: MatDialogRef<ModalConveniosComponent>,
    private notificationService: NotificationService,
    @Inject(MAT_DIALOG_DATA) public data: ModalData
  ) {}

  ngOnInit(): void {
    this.isEdit = !!this.data?.data.isEdit;
    this.initFormCadastro();
  }

  initFormCadastro() {
    const object: Convenio = this.data.element;

    this.formCadastro = this.formBuilder.group({
      id: [object?.id || null],
      nome: [object?.nome || '', Validators.required],
      status: [{value: object?.status}, Validators.required],
      idMunicipio: [object?.idMunicipio ? { value: object.idMunicipio } : null, Validators.required],
      observacao: [object?.observacao || ''],
      tipoConvenio: [{value: object?.tipoConvenio}, Validators.required],
    });

    if (this.data.isVisualizacao) {
      this.formCadastro.disable();
    }
  }

  valueFromForm(): Convenio {
    const valor = this.formCadastro.value;
    console.log(valor)
    return {...valor } as Convenio;
  }

  onConfirm(): void {
    console.log(this.valueFromForm())
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
      this.convenioService.editar(this.valueFromForm()).subscribe((val) => {
        this.notificationService.showSuccess('Convênio editado com sucesso!');
        this.dialogRef.close();
      });
    } else {
      this.convenioService.salvar(this.valueFromForm()).subscribe((val) => {
        this.notificationService.showSuccess('Convênio salvo com sucesso!');
        this.dialogRef.close();
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }
}
