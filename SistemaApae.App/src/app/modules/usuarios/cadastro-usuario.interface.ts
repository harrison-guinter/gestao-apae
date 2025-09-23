export interface CadastroUsuario {
  id: string;
  nome: string;
  email: string;
  tipo: 'Coordenador' | 'Profissional';
  especialidade?: string;
  ativo: boolean;
  orgaoClasse?: string;
  registroProfissional?: string;
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
