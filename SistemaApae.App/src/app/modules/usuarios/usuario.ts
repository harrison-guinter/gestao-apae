import { Roles } from '../auth/roles.enum';

export class Usuario {
  id: string;
  name: string;
  email: string;
  perfil: Roles;
  ativo: boolean;
  especialidade?: string;
  observacoes?: string;
  registroProfissional?: string;
  telefone?: string;

  constructor(
    id: string,
    name: string,
    email: string,
    perfil: Roles,
    ativo: boolean,
    especialidade?: string,
    observacoes?: string,
    registroProfissional?: string,
    telefone?: string
  ) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.perfil = perfil;
    this.especialidade = especialidade;
    this.ativo = ativo;
    this.observacoes = observacoes;
    this.registroProfissional = registroProfissional; // orgão classe + número do registro
    this.telefone = telefone;
  }

  hasRole(role: Roles): boolean {
    // TODO
    return true;
    return this.perfil == role;
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
      jsonParsed.observacoes,
      jsonParsed.registroProfissional,
      jsonParsed.telefone
    );
  }
}

//apae@apae
//paulo1234
