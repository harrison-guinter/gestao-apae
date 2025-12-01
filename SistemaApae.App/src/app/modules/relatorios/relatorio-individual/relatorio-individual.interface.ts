export interface RelatorioIndividual {
  atendimento: {
    id: string;
    data: string;
  };
  profissional: {
    id: string;
    nome: string;
  };
  assistido: {
    id: string;
    nome: string;
  };
  municipio: {
    id: string;
    nome: string;
  };
}
