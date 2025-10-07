import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { InputComponent } from '../../core/input/input.component';
import { SelectComponent, SelectOption } from '../../core/select/select.component';
import { DateComponent } from '../../core/date/date.component';
import { PageInfoService } from '../../core/services/page-info.service';
import { NotificationService } from '../../core/notification/notification.service';
import { AssistidoService } from '../assistido.service';
import {
  Assistido,
  StatusAssistidoEnum,
  SexoEnum,
  TipoDeficienciaEnum,
  PlanoSaudeEnum,
  TurnoEscolaEnum,
} from '../assistido';

@Component({
  selector: 'app-cadastro-assistido',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatNativeDateModule,
    InputComponent,
    SelectComponent,
    DateComponent,
  ],
  templateUrl: './cadastro-assistido.component.html',
  styleUrls: ['./cadastro-assistido.component.less'],
})
export class CadastroAssistidoComponent implements OnInit {
  protected formCadastro!: UntypedFormGroup;
  protected isEdit: boolean = false;
  protected assistidoId?: string;

  // Options para selects
  statusOptions: SelectOption[] = [
    { value: StatusAssistidoEnum.ATIVO, label: 'Ativo' },
    { value: StatusAssistidoEnum.INATIVO, label: 'Inativo' },
  ];

  sexoOptions: SelectOption[] = [
    { value: SexoEnum.MASCULINO, label: 'Masculino' },
    { value: SexoEnum.FEMININO, label: 'Feminino' },
  ];

  tipoDeficienciaOptions: SelectOption[] = [
    { value: TipoDeficienciaEnum.INTELECTUAL, label: 'Intelectual' },
    { value: TipoDeficienciaEnum.FISICA, label: 'Física' },
    { value: TipoDeficienciaEnum.AUDITIVA, label: 'Auditiva' },
    { value: TipoDeficienciaEnum.VISUAL, label: 'Visual' },
    { value: TipoDeficienciaEnum.MULTIPLA, label: 'Múltipla' },
    { value: TipoDeficienciaEnum.AUTISMO, label: 'Autismo' },
  ];

  planoSaudeOptions: SelectOption[] = [
    { value: PlanoSaudeEnum.SUS, label: 'SUS' },
    { value: PlanoSaudeEnum.PARTICULAR, label: 'Particular' },
    { value: PlanoSaudeEnum.CONVENIO, label: 'Convênio' },
  ];

  turnoEscolaOptions: SelectOption[] = [
    { value: TurnoEscolaEnum.MATUTINO, label: 'Matutino' },
    { value: TurnoEscolaEnum.VESPERTINO, label: 'Vespertino' },
    { value: TurnoEscolaEnum.NOTURNO, label: 'Noturno' },
    { value: TurnoEscolaEnum.INTEGRAL, label: 'Integral' },
  ];

  constructor(
    private formBuilder: UntypedFormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private pageInfoService: PageInfoService,
    private notificationService: NotificationService,
    private assistidoService: AssistidoService
  ) {}

  ngOnInit(): void {
    this.initFormCadastro();
    this.checkEditMode();
  }

  private checkEditMode(): void {
    this.assistidoId = this.route.snapshot.paramMap.get('id') || undefined;
    this.isEdit = !!this.assistidoId;

    if (this.isEdit) {
      this.pageInfoService.updatePageInfo('Editar Assistido', 'Editar dados do assistido');
      this.carregarAssistido();
    } else {
      this.pageInfoService.updatePageInfo('Cadastrar Assistido', 'Cadastrar novo assistido');
    }
  }

  private carregarAssistido(): void {
    if (this.assistidoId) {
      this.assistidoService.obterAssistido(this.assistidoId).subscribe({
        next: (assistido) => {
          this.preencherFormulario(assistido);
        },
        error: (error) => {
          this.notificationService.showError('Erro ao carregar assistido');
          this.router.navigate(['/assistidos']);
        },
      });
    }
  }

  private preencherFormulario(assistido: Assistido): void {
    this.formCadastro.patchValue({
      nome: assistido.nome,
      dataNascimento: assistido.dataNascimento ? new Date(assistido.dataNascimento) : null,
      cpf: assistido.cpf,
      sexo: assistido.sexo,
      endereco: assistido.endereco,
      bairro: assistido.bairro,
      cep: assistido.cep,
      naturalidade: assistido.naturalidade,
      nomeMae: assistido.nomeMae,
      nomePai: assistido.nomePai,
      nomeResponsavel: assistido.nomeResponsavel,
      telefoneResponsavel: assistido.telefoneResponsavel,
      tipoDeficiencia: assistido.tipoDeficiencia,
      cid: assistido.cid,
      medicamentosUso: assistido.medicamentosUso,
      medicamentosQuais: assistido.medicamentosQuais,
      status: assistido.status,
      observacao: assistido.observacao,
    });
  }

  initFormCadastro(): void {
    this.formCadastro = this.formBuilder.group({
      nome: ['', Validators.required],
      dataNascimento: [null],
      cpf: [''],
      sexo: [null],
      endereco: [''],
      bairro: [''],
      cep: [''],
      naturalidade: [''],
      nomeMae: [''],
      nomePai: [''],
      nomeResponsavel: [''],
      telefoneResponsavel: [''],
      tipoDeficiencia: [null],
      cid: [''],
      medicamentosUso: [false],
      medicamentosQuais: [''],
      status: [StatusAssistidoEnum.ATIVO, Validators.required],
      observacao: [''],
    });
  }

  onSubmit(): void {
    if (this.formCadastro.invalid) {
      this.formCadastro.markAllAsTouched();
      this.formCadastro.updateValueAndValidity();

      this.notificationService.showWarning(
        'Campos obrigatórios não preenchidos. Verifique os campos destacados.'
      );
      return;
    }

    const assistidoData: Assistido = {
      ...this.formCadastro.value,
      id: this.assistidoId,
      dataNascimento: this.formCadastro.value.dataNascimento
        ? this.formCadastro.value.dataNascimento.toISOString().split('T')[0]
        : null,
    };

    if (this.isEdit) {
      this.assistidoService.editarAssistido(assistidoData).subscribe({
        next: () => {
          this.notificationService.showSuccess('Assistido editado com sucesso!');
          this.router.navigate(['/home/assistidos']);
        },
        error: (error) => {
          this.notificationService.showError('Erro ao editar assistido');
        },
      });
    } else {
      this.assistidoService.salvarAssistido(assistidoData).subscribe({
        next: () => {
          this.notificationService.showSuccess('Assistido cadastrado com sucesso!');
          this.router.navigate(['/home/assistidos']);
        },
        error: (error) => {
          this.notificationService.showError('Erro ao cadastrar assistido');
        },
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/home/assistidos']);
  }
}
