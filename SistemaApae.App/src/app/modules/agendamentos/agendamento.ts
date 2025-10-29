import { Assistido } from '../assistidos/assistido';
import { Status } from '../core/enum/status.enum';
import { Usuario } from '../usuarios/usuario';

export class Agendamento {
  id: string;
  nome: string;
  observacao: string;
  status: Status;
  assistidos: Assistido[]; 
  profissional: Usuario;
  tipoRecorrencia: TipoRecorrencia;
  dataAgendamento: Date;
  horarioAgendamento: string;
  diaSemana?: DiaDaSemana;

    constructor(
        id: string,
        nome: string,
        observacao: string,
        status: Status,
        assistidos: Assistido[],
        profissional: Usuario,
        tipoRecorrencia: TipoRecorrencia,
        dataAgendamento: Date,
        horarioAgendamento: string,
        diaSemana?: DiaDaSemana
    ) {
        this.id = id;
        this.nome = nome;
        this.observacao = observacao;
        this.status = status;
        this.assistidos = assistidos;
        this.profissional = profissional;
        this.tipoRecorrencia = tipoRecorrencia;
        this.dataAgendamento = new Date(dataAgendamento);
        this.horarioAgendamento = horarioAgendamento;
        this.diaSemana = diaSemana;
    }
}

export enum TipoRecorrencia {
  NENHUM = 'NENHUM',
  SEMANAL = 'SEMANAL',
}

export enum DiaDaSemana {
    SEGUNDA = 'SEGUNDA',
    TERCA = 'TERCA',
    QUARTA = 'QUARTA',
    QUINTA = 'QUINTA',
    SEXTA = 'SEXTA',
    SABADO = 'SABADO',
}


