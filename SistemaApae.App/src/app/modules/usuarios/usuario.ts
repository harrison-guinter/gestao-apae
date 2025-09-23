import { Roles } from '../auth/roles.enum';

export class Usuario {
  id: string;
  name: string;
  email: string;
  perfil: Roles;
  especialidade?: string;
  ativo: boolean;
  observacoes?: string;
  orgaoClasse?: OrgaoClasse;
  registroProfissional?: string;

  constructor(
    id: string,
    name: string,
    email: string,
    perfil: Roles,
    especialidade: string,
    ativo: boolean,
    observacoes?: string,
    orgaoClasse?: OrgaoClasse,
    registroProfissional?: string
  ) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.perfil = perfil;
    this.especialidade = especialidade;
    this.ativo = ativo;
    this.observacoes = observacoes;
    this.orgaoClasse = orgaoClasse;
    this.registroProfissional = registroProfissional;
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
      jsonParsed.orgaoClasse,
      jsonParsed.registroProfissional
    );
  }
}

export enum OrgaoClasse {
  CRP = 'CRP',
  CREFITO = 'CREFITO',
  CFM = 'CFM',
  CRO = 'CRO',
  CRM = 'CRM',
  COREME = 'COREME',
  CRN = 'CRN',
}

//apae@apae
//paulo1234
