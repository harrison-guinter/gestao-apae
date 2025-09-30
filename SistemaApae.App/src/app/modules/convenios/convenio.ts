import { Cidade } from "../cidades/cidade";

export class Convenio {
    id: string;
    nome: string;
    municipio: Cidade;
    idMunicipio: string;
    observacao: string;
    status: number;
    tipoConvenio: number;

    constructor(id: string, nome: string, municipio: Cidade, observacao: string, status: number, tipoConvenio: number) {
        this.id = id;
        this.nome = nome;
        this.municipio = municipio;
        this.observacao = observacao;
        this.status = status;
        this.tipoConvenio = tipoConvenio;
        this.idMunicipio = municipio.id;
    }
}