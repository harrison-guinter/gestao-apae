import { Roles } from '../auth/roles.enum';

export enum StatusUsuarioEnum {
  ATIVO = 'Ativo',
  INATIVO = 'Inativo',
}

export class Usuario {
  id!: string;
  nome!: string;
  email!: string;
  perfil!: Roles;
  status!: StatusUsuarioEnum;
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
    status: StatusUsuarioEnum,
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
    status?: StatusUsuarioEnum,
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
    return this.status === StatusUsuarioEnum.ATIVO;
  }

  getStatusLabel(): string {
    return this.status === StatusUsuarioEnum.ATIVO ? 'Ativo' : 'Inativo';
  }

  static getCurrentUser(): Usuario {
    const jsonParsed = JSON.parse(localStorage.getItem('usuario') || '{}');

    return new Usuario({
      id: jsonParsed.id,
      nome: jsonParsed.name, 
      email: jsonParsed.email,
      perfil: jsonParsed.perfil,
      status: jsonParsed.ativo ? StatusUsuarioEnum.ATIVO : StatusUsuarioEnum.INATIVO,
      especialidade: jsonParsed.especialidade,
      observacao: jsonParsed.observacao,
      registroProfissional: jsonParsed.registroProfissional,
      telefone: jsonParsed.telefone,
    });
  }
}

//apae@apae 
//paulo1234
