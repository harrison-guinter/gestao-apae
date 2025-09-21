export interface CadastroUsuario {
  id: string;
  nome: string;
  email: string;
  tipo: 'Coordenador' | 'Profissional';
  especialidade: string;
  ativo: boolean;
}
