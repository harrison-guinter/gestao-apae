import { Cidade } from "../cidades/cidade";

export class Convenio {
    Id: string;
    Nome: string;
    Municipio: Cidade;
    IdMunicipio: string;
    Observacao: string;
    Status: number;
    TipoConvenio: number;

    constructor(Id: string, Nome: string, Municipio: Cidade, Observacao: string, Status: number, TipoConvenio: number) {
        this.Id = Id;
        this.Nome = Nome;
        this.Municipio = Municipio;
        this.Observacao = Observacao;
        this.Status = Status;
        this.TipoConvenio = TipoConvenio;
        this.IdMunicipio = Municipio.Id;
    }
}