import { Roles } from '../auth/roles.enum';
import { Status } from '../core/enum/status.enum';

export class Usuario {
  id!: string;
  nome!: string;
  email!: string;
  perfil!: Roles;
  status!: Status;
  especialidade?: string;
  observacao?: string;
  registroProfissional?: string;
  telefone?: string;
  UpdatedAt?: Date;

  constructor();
  constructor(data: Partial<Usuario>);
  constructor(
    id: string,
    nome: string,
    email: string,
    perfil: Roles,
    status: Status,
    especialidade?: string,
    observacao?: string,
    registroProfissional?: string,
    telefone?: string
  );

  // 👉 Implementação única
  constructor(
    idOrData?: string | Partial<Usuario>,
    nome?: string,
    email?: string,
    perfil?: Roles,
    status?: Status,
    especialidade?: string,
    observacao?: string,
    registroProfissional?: string,
    telefone?: string
  ) {
    if (typeof idOrData === 'object') {
      Object.assign(this, idOrData);
    } else if (idOrData) {
      this.id = idOrData;
      this.nome = nome!;
      this.email = email!;
      this.perfil = perfil!;
      this.status = status!;
      this.especialidade = especialidade;
      this.observacao = observacao;
      this.registroProfissional = registroProfissional;
      this.telefone = telefone;
    }
  }

  hasRole(role: Roles): boolean {
    return true;
    return this.perfil === role;
  }

  isActive(): boolean {
    return this.status === Status.Ativo;
  }

  static getCurrentUser(): Usuario {
    const jsonParsed = JSON.parse(localStorage.getItem('usuario') || '{}');

    return new Usuario({
      id: jsonParsed.id,
      nome: jsonParsed.name,
      email: jsonParsed.email,
      perfil: jsonParsed.perfil,
      status: jsonParsed.ativo ? Status.Ativo : Status.Inativo,
      especialidade: jsonParsed.especialidade,
      observacao: jsonParsed.observacao,
      registroProfissional: jsonParsed.registroProfissional,
      telefone: jsonParsed.telefone,
    });
  }
}

//apae@apae
//paulo1234

// Login: mipedi7767@dwakm.com
// Senha: swVIX&aTvO3y
