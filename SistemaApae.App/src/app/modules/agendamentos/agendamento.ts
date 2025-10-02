import { Status } from '../core/enum/status.enum';
import { Usuario } from '../usuarios/usuario';

export class Agendamento {
  id: string;
  nome: string;
  observacao: string;
  status: Status;
  assistidos: any[]; // TODO assistidos 
  profissional: Usuario;
  tipoRecorrencia: TipoRecorrencia;
  data: Date;
  hora: string;
  diaDaSemana: DiaDaSemana;

    constructor(
        id: string,
        nome: string,
        observacao: string,
        status: Status,
        assistidos: any[],
        profissional: Usuario,
        tipoRecorrencia: TipoRecorrencia,
        data: Date,
        hora: string,
        diaDaSemana: DiaDaSemana
    ) {
        this.id = id;
        this.nome = nome;
        this.observacao = observacao;
        this.status = status;
        this.assistidos = assistidos;
        this.profissional = profissional;
        this.tipoRecorrencia = tipoRecorrencia;
        this.data = data;
        this.hora = hora;
        this.diaDaSemana = diaDaSemana;
    }
}

export enum TipoRecorrencia {
  NENHUM = 'NENHUM',
  SEMANAL = 'SEMANAL',
}

export enum DiaDaSemana {
    DOMINGO = 'DOMINGO',
    SEGUNDA = 'SEGUNDA',
    TERCA = 'TERCA',
    QUARTA = 'QUARTA',
    QUINTA = 'QUINTA',
    SEXTA = 'SEXTA',
    SABADO = 'SABADO',
}


