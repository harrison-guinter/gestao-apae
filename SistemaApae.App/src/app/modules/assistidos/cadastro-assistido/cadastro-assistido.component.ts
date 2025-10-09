import { Component, inject, OnInit } from '@angular/core';
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
import { CidadesService } from '../../cidades/cidades.service';
import { ConvenioService } from '../../convenios/convenio.service';
import { map } from 'rxjs/internal/operators/map';

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

  private cidadesService: CidadesService = inject(CidadesService);
  private convenioService: ConvenioService = inject(ConvenioService);

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

  cidades$ = this.cidadesService
    .listarCidades()
    .pipe(map((cidades) => cidades.map((cidade) => ({ value: cidade.id, label: cidade.nome }))));

  convenios$ = this.convenioService
    .listarConvenios({} as any)
    .pipe(
      map((convenios) =>
        convenios.map((convenio) => ({ value: convenio.id, label: convenio.nome }))
      )
    );

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
      // Dados Pessoais
      nome: assistido.nome,
      dataNascimento: assistido.dataNascimento ? new Date(assistido.dataNascimento) : null,
      cpf: assistido.cpf,
      sexo: assistido.sexo,
      naturalidade: assistido.naturalidade,
      nomeMae: assistido.nomeMae,
      nomePai: assistido.nomePai,

      // Endereço
      endereco: assistido.endereco,
      bairro: assistido.bairro,
      cep: assistido.cep,
      idMunicipio: assistido.idMunicipio,

      // Responsável
      nomeResponsavel: assistido.nomeResponsavel,
      telefoneResponsavel: assistido.telefoneResponsavel,
      responsavelBusca: assistido.responsavelBusca,

      // Saúde
      idConvenio: assistido.idConvenio,
      tipoDeficiencia: assistido.tipoDeficiencia,
      cid: assistido.cid,
      medicamentosUso: assistido.medicamentosUso,
      medicamentosQuais: assistido.medicamentosQuais,
      planoSaude: assistido.planoSaude,

      // Escolaridade
      nomeEscola: assistido.nomeEscola,
      anoEscola: assistido.anoEscola,
      turnoEscola: assistido.turnoEscola,
      acompanhamentoEspecializado: assistido.acompanhamentoEspecializado,

      // Benefícios Sociais
      bpc: assistido.bpc,
      bolsaFamilia: assistido.bolsaFamilia,
      passeLivreEstadual: assistido.passeLivreEstadual,
      passeLivreMunicipal: assistido.passeLivreMunicipal,
      composicaoFamiliar: assistido.composicaoFamiliar,

      // Informações Familiares
      paisCasados: assistido.paisCasados,
      paternidadeRegistrada: assistido.paternidadeRegistrada,
      consentimentoImagem: assistido.consentimentoImagem,

      // Desenvolvimento e Comportamento
      boaSocializacao: assistido.boaSocializacao,
      boaAdaptacao: assistido.boaAdaptacao,
      comportamentoAgressivo: assistido.comportamentoAgressivo,
      controleEsfincteres: assistido.controleEsfincteres,
      apegoFamiliar: assistido.apegoFamiliar,
      caracteristicasMarcantes: assistido.caracteristicasMarcantes,

      // Atrasos no Desenvolvimento
      atrasoAlimentacao: assistido.atrasoAlimentacao,
      atrasoHigiene: assistido.atrasoHigiene,
      atrasoVestuario: assistido.atrasoVestuario,
      atrasoLocomocao: assistido.atrasoLocomocao,
      atrasoComunicacao: assistido.atrasoComunicacao,

      // Histórico Médico e Gestacional
      descricaoGestacao: assistido.descricaoGestacao,
      usoMedicacaoMae: assistido.usoMedicacaoMae,
      gestacaoSemanas: assistido.gestacaoSemanas,
      internacaoPosNascimento: assistido.internacaoPosNascimento,
      medicoResponsavel: assistido.medicoResponsavel,
      examesRealizados: assistido.examesRealizados,
      doencasFisicas: assistido.doencasFisicas,
      qualidadeSono: assistido.qualidadeSono,
      cirurgiasRealizadas: assistido.cirurgiasRealizadas,
      doencasNeurologicas: assistido.doencasNeurologicas,
      historicoFamiliarDoencas: assistido.historicoFamiliarDoencas,

      // Outros
      descricaoDemanda: assistido.descricaoDemanda,
      status: assistido.status,
      observacao: assistido.observacao,
    });
  }

  initFormCadastro(): void {
    this.formCadastro = this.formBuilder.group({
      // Dados Pessoais
      nome: ['', Validators.required],
      dataNascimento: [null],
      cpf: [''],
      sexo: [null],
      naturalidade: [''],
      nomeMae: [''],
      nomePai: [''],

      // Endereço
      endereco: [''],
      bairro: [''],
      cep: [''],
      idMunicipio: [null],

      // Responsável
      nomeResponsavel: [''],
      telefoneResponsavel: [''],
      responsavelBusca: [''],

      // Saúde
      idConvenio: [null],
      tipoDeficiencia: [null],
      cid: [''],
      medicamentosUso: [false],
      medicamentosQuais: [''],
      planoSaude: [null],

      // Escolaridade
      nomeEscola: [''],
      anoEscola: [''],
      turnoEscola: [null],
      acompanhamentoEspecializado: [false],

      // Benefícios Sociais
      bpc: [false],
      bolsaFamilia: [false],
      passeLivreEstadual: [false],
      passeLivreMunicipal: [false],
      composicaoFamiliar: [''],

      // Informações Familiares
      paisCasados: [false],
      paternidadeRegistrada: [false],
      consentimentoImagem: [false],

      // Desenvolvimento e Comportamento
      boaSocializacao: [false],
      boaAdaptacao: [false],
      comportamentoAgressivo: [false],
      controleEsfincteres: [false],
      apegoFamiliar: [''],
      caracteristicasMarcantes: [''],

      // Atrasos no Desenvolvimento
      atrasoAlimentacao: [false],
      atrasoHigiene: [false],
      atrasoVestuario: [false],
      atrasoLocomocao: [false],
      atrasoComunicacao: [false],

      // Histórico Médico e Gestacional
      descricaoGestacao: [''],
      usoMedicacaoMae: [''],
      gestacaoSemanas: [null],
      internacaoPosNascimento: [false],
      medicoResponsavel: [''],
      examesRealizados: [''],
      doencasFisicas: [''],
      qualidadeSono: [''],
      cirurgiasRealizadas: [''],
      doencasNeurologicas: [''],
      historicoFamiliarDoencas: [''],

      // Outros
      descricaoDemanda: [''],
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
      this.assistidoService.editarAssistido(assistidoData).subscribe((val) => {
        this.notificationService.showSuccess('Assistido editado com sucesso!');
        this.router.navigate(['/home/assistidos']);
      });
    } else {
      this.assistidoService.salvarAssistido(assistidoData).subscribe((val) => {
        this.notificationService.showSuccess('Assistido cadastrado com sucesso!');
        this.router.navigate(['/home/assistidos']);
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/home/assistidos']);
  }
}
