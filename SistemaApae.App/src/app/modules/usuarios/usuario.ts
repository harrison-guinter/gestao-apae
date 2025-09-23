import { Roles } from "../auth/roles.enum";

export class Usuario {
  id: string;
  name: string;
  email: string;
  perfil: Roles;
  especialidade: string;
  ativo: boolean;

  constructor(id: string, name: string, email: string, perfil: Roles, especialidade: string, ativo: boolean) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.perfil = perfil;
    this.especialidade = especialidade;
    this.ativo = ativo;
  }

  hasRole(role: Roles): boolean {
    // TODO
    return true
    return this.perfil == role;
  }

  static getCurrentUser(): Usuario {
    const jsonParsed = JSON.parse(localStorage.getItem('usuario') || "{}");         
 
    return new Usuario(
      jsonParsed.id,
      jsonParsed.name,
      jsonParsed.email,
      jsonParsed.roles,
      jsonParsed.especialidade,
      jsonParsed.ativo
    );
  }
}

//apae@apae
//paulo1234
