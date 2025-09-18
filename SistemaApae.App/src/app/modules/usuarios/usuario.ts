export class Usuario {
  id: string;
  name: string;
  email: string;
  roles: string[];

  constructor(id: string, name: string, email: string, roles: string[]) {
    this.id = id;
    this.name = name;
    this.email = email;
    this.roles = roles;
  }

  hasRole(role: string): boolean {
    return this.roles.includes(role);
  }
}
