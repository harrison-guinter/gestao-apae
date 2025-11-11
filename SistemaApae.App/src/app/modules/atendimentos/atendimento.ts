import { Status } from '../core/enum/status.enum';
import { Assistido } from '../assistidos/assistido';
import { Agendamento } from '../agendamentos/agendamento';
import { Usuario } from '../usuarios/usuario';

export enum StatusAtendimentoEnum {
  PRESENTE = 'PRESENCA',
  FALTA = 'FALTA',
  FALTA_JUSTIFICADA = 'JUSTIFICADA',
}

export class Atendimento {
  id: string;
  profissional: Usuario;
  assistido: Assistido;
  dataAtendimento?: Date;
  presenca?: StatusAtendimentoEnum;
  avaliacao?: string;
  observacao?: string;
  status: Status;
  createdAt?: Date;
  updatedAt?: Date;

  constructor(data: any = {}) {
    this.id = data.id || '';
    this.profissional = data.profissional || data.profissional || '';
    this.assistido = data.assistido || data.assistido || '';
    this.dataAtendimento = data.dataAtendimento
      ? new Date(data.dataAtendimento)
      : data.data_atendimento
      ? new Date(data.data_atendimento)
      : undefined;
    this.presenca = data.presenca !== undefined ? data.presenca : undefined;
    this.avaliacao = data.avaliacao || '';
    this.observacao = data.observacao || '';
    this.status = data.status !== undefined ? data.status : Status.Ativo;
    this.createdAt = data.createdAt
      ? new Date(data.createdAt)
      : data.created_at
      ? new Date(data.created_at)
      : undefined;
    this.updatedAt = data.updatedAt
      ? new Date(data.updatedAt)
      : data.updated_at
      ? new Date(data.updated_at)
      : undefined;

    // Relacionamentos
    this.assistido = data.assistido || undefined;
    this.profissional = data.profissional || undefined;
  }

  get presencaTexto(): string {
    switch (this.presenca) {
      case StatusAtendimentoEnum.PRESENTE:
        return 'Presente';
      case StatusAtendimentoEnum.FALTA:
        return 'Falta';
      case StatusAtendimentoEnum.FALTA_JUSTIFICADA:
        return 'Falta Justificada';
      default:
        return 'Não informado';
    }
  }

  get statusTexto(): string {
    return this.status === Status.Ativo ? 'Ativo' : 'Inativo';
  }
}
