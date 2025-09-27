import { Roles } from '../auth/roles.enum';

export enum StatusUsuarioEnum {
  ATIVO = 1,
  INATIVO = 2,
}

export class Usuario {
  id: string;
  nome: string;
  email: string;
  perfil: Roles;
  status: StatusUsuarioEnum;
  especialidade?: string;
  observacao?: string;
  registroProfissional?: string;
  telefone?: string;

  constructor(
    id: string,
    nome: string,
    email: string,
    perfil: Roles,
    status: StatusUsuarioEnum,
    especialidade?: string,
    observacao?: string,
    registroProfissional?: string,
    telefone?: string
  ) {
    this.id = id;
    this.nome = nome;
    this.email = email;
    this.perfil = perfil;
    this.especialidade = especialidade;
    this.status = status;
    this.observacao = observacao;
    this.registroProfissional = registroProfissional; // orgão classe + número do registro
    this.telefone = telefone;
  }

  hasRole(role: Roles): boolean {
    // TODO
    return true;
    return this.perfil == role;
  }

  isActive(): boolean {
    return this.status === StatusUsuarioEnum.ATIVO;
  }

  getStatusLabel(): string {
    return this.status === StatusUsuarioEnum.ATIVO ? 'Ativo' : 'Inativo';
  }

  static getCurrentUser(): Usuario {
    const jsonParsed = JSON.parse(localStorage.getItem('usuario') || '{}');

    return new Usuario(
      jsonParsed.id,
      jsonParsed.name,
      jsonParsed.email,
      jsonParsed.roles,
      jsonParsed.especialidade,
      jsonParsed.ativo,
      jsonParsed.observacao,
      jsonParsed.registroProfissional,
      jsonParsed.telefone
    );
  }
}

//apae@apae
//paulo1234
