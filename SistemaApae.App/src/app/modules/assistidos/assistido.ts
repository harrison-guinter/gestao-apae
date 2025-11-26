export interface Assistido {
  id?: string;
  nome: string;
  dataNascimento?: string;
  endereco?: string;
  observacao?: string;
  idConvenio?: string;
  status: StatusAssistidoEnum;
  dataCadastro?: string;
  naturalidade?: string;
  sexo?: SexoEnum;
  tipoDeficiencia?: TipoDeficienciaEnum;
  cid?: string;
  medicamentosUso: boolean;
  medicamentosQuais?: string;
  nomeResponsavel?: string;
  telefoneResponsavel?: string;
  cpf?: string;
  bairro?: string;
  idMunicipio?: string;
  cep?: string;
  nomeMae?: string;
  nomePai?: string;
  descricaoDemanda?: string;
  bpc?: boolean;
  bolsaFamilia?: boolean;
  passeLivreEstadual?: boolean;
  passeLivreMunicipal?: boolean;
  composicaoFamiliar?: string;
  planoSaude?: PlanoSaudeEnum;
  descricaoGestacao?: string;
  usoMedicacaoMae?: string;
  gestacaoSemanas?: number;
  internacaoPosNascimento?: boolean;
  medicoResponsavel?: string;
  examesRealizados?: string;
  doencasFisicas?: string;
  qualidadeSono?: string;
  cirurgiasRealizadas?: string;
  doencasNeurologicas?: string;
  historicoFamiliarDoencas?: string;
  caracteristicasMarcantes?: string;
  boaSocializacao?: boolean;
  boaAdaptacao?: boolean;
  comportamentoAgressivo?: boolean;
  controleEsfincteres?: boolean;
  apegoFamiliar?: string;
  atrasoAlimentacao?: boolean;
  atrasoHigiene?: boolean;
  atrasoVestuario?: boolean;
  atrasoLocomocao?: boolean;
  atrasoComunicacao?: boolean;
  acompanhamentoEspecializado?: boolean;
  nomeTurmaApae?: string;
  turnoTurmaApae?: TurnoEscolaEnum;
  diasTurmaApae?: string[];
  nomeEscola?: string;
  anoEscola?: string;
  turnoEscola?: TurnoEscolaEnum;
  paisCasados?: boolean;
  paternidadeRegistrada?: boolean;
  responsavelBusca?: string;
  consentimentoImagem?: boolean;
}

export enum StatusAssistidoEnum {
  ATIVO = 'Ativo',
  INATIVO = 'Inativo',
}

export enum SexoEnum {
  MASCULINO = 'M',
  FEMININO = 'F',
}

export enum TipoDeficienciaEnum {
  INTELECTUAL = 'INTELECTUAL',
  FISICA = 'FISICA',
  AUDITIVA = 'AUDITIVA',
  VISUAL = 'VISUAL',
  MULTIPLA = 'MULTIPLA',
  AUTISMO = 'AUTISMO',
}

export enum PlanoSaudeEnum {
  SUS = 1,
  PARTICULAR = 2,
  CONVENIO = 3,
}

export enum TurnoEscolaEnum {
  MANHA = 'MANHA',
  TARDE = 'TARDE',
  NOITE = 'NOITE',
  INTEGRAL = 'INTEGRAL',
}
