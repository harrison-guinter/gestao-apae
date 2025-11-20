import { Agendamento, DiaDaSemana, TipoRecorrencia } from "../../agendamentos/agendamento";
import { Assistido } from "../../assistidos/assistido";
import { Usuario } from "../../usuarios/usuario";

export interface AtendimentoPendente {
  assistido: Assistido;
  dataAgendamento: Date;
  diaSemana: DiaDaSemana
  horarioAgendamento: string;
  tipoRecorrencia: TipoRecorrencia;
  profissional: Usuario;
  agendamento: Agendamento;
}