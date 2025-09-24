import { Cidade } from "../cidades/cidade";

export class Convenio {
    id: string;
    nome: string;
    cidade: Cidade;
    observacoes: string;
    ativo: boolean;

    constructor(id: string, nome: string, cidade: Cidade, observacoes: string, ativo: boolean) {
        this.id = id;
        this.nome = nome;
        this.cidade = cidade;
        this.observacoes = observacoes;
        this.ativo = ativo;
    }
}