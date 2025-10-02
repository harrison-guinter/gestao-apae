import { Cidade } from "../cidades/cidade";
import { Status } from "../core/enum/status.enum";

export class Convenio {
    id: string;
    nome: string;
    municipio: Cidade;
    idMunicipio: string;
    observacao: string;
    status: Status;
    tipoConvenio: number;

    constructor(id: string, nome: string, municipio: Cidade, observacao: string, status: Status, tipoConvenio: number) {
        this.id = id;
        this.nome = nome;
        this.municipio = municipio;
        this.observacao = observacao;
        this.status = status;
        this.tipoConvenio = tipoConvenio;
        this.idMunicipio = municipio.id;
    }
}

export enum TipoConvenio {
    CAS = 'CAS',
    Educacao = 'EDUCACAO',
    Saude = 'SAUDE',
    AssistenciaSocial = 'ASSISTENCIA_SOCIAL',
    EJA = 'EJA'
}