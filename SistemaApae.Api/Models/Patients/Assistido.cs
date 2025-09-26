using SistemaApae.Api.Models.Administrative;
using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Models.Enums;
using Supabase.Postgrest.Attributes;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de assistido do sistema
/// </summary>
[Table("assistido")]
public class Assistido : ApiBaseModel
{

    /// <summary>
    /// Nome do assistido
    /// </summary>
    [Required]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    /// <summary>
    /// Data de nascimento do assistido
    /// </summary>
    [Column("data_nascimento")]
    public DateOnly? DataNascimento { get; set; }

    /// <summary>
    /// Endereço do assistido
    /// </summary>
    [Column("endereco")]
    public string? Endereco { get; set; }

    /// <summary>
    /// Observações sobre o assistido
    /// </summary>
    [Column("observacao")]
    public string? Observacao { get; set; }

    /// <summary>
    /// ID do convênio associado
    /// </summary>
    [Column("id_convenio")]
    public Guid? IdConvenio { get; set; }

    /// <summary>
    /// Status do assistido ativo/inativo
    /// </summary>
    [Column("status")]
    public StatusEntidadeEnum Status { get; set; }

    /// <summary>
    /// Data de cadastro do assistido
    /// </summary>
    [Column("data_cadastro")]
    public DateOnly? DataCadastro { get; set; }

    /// <summary>
    /// Naturalidade do assistido
    /// </summary>
    [Column("naturalidade")]
    public string? Naturalidade { get; set; }

    /// <summary>
    /// Sexo do assistido (M/F)
    /// </summary>
    [Column("sexo")]
    public SexoEnum? Sexo { get; set; }

    /// <summary>
    /// Tipo de deficiência
    /// </summary>
    [Column("tipo_deficiencia")]
    public TipoDeficienciaEnum? TipoDeficiencia { get; set; }

    /// <summary>
    /// CID do assistido
    /// </summary>
    [Column("cid")]
    public string? Cid { get; set; }

    /// <summary>
    /// Indica se faz uso de medicamentos
    /// </summary>
    [Column("medicamentos_uso")]
    public bool MedicamentosUso { get; set; } = false;

    /// <summary>
    /// Quais medicamentos utiliza
    /// </summary>
    [Column("medicamentos_quais")]
    public string? MedicamentosQuais { get; set; }

    /// <summary>
    /// Nome do responsável
    /// </summary>
    [Column("nome_responsavel")]
    public string? NomeResponsavel { get; set; }

    /// <summary>
    /// Telefone do responsável
    /// </summary>
    [Column("telefone_responsavel")]
    public string? TelefoneResponsavel { get; set; }

    /// <summary>
    /// CPF do assistido
    /// </summary>
    [Column("cpf")]
    public string? Cpf { get; set; }

    /// <summary>
    /// Bairro do endereço
    /// </summary>
    [Column("bairro")]
    public string? Bairro { get; set; }

    /// <summary>
    /// ID do município
    /// </summary>
    [Column("id_municipio")]
    public Guid? IdMunicipio { get; set; }

    /// <summary>
    /// CEP do endereço
    /// </summary>
    [Column("cep")]
    public string? Cep { get; set; }

    /// <summary>
    /// Nome da mãe
    /// </summary>
    [Column("nome_mae")]
    public string? NomeMae { get; set; }

    /// <summary>
    /// Nome do pai
    /// </summary>
    [Column("nome_pai")]
    public string? NomePai { get; set; }

    /// <summary>
    /// Descrição da demanda
    /// </summary>
    [Column("descricao_demanda")]
    public string? DescricaoDemanda { get; set; }

    /// <summary>
    /// Benefício de Prestação Continuada (BPC)
    /// </summary>
    [Column("bpc")]
    public bool? Bpc { get; set; }

    /// <summary>
    /// Indica se possui Bolsa Família
    /// </summary>
    [Column("bolsa_familia")]
    public bool? BolsaFamilia { get; set; }

    /// <summary>
    /// Possui passe livre estadual
    /// </summary>
    [Column("passe_livre_estadual")]
    public bool? PasseLivreEstadual { get; set; }

    /// <summary>
    /// Possui passe livre municipal
    /// </summary>
    [Column("passe_livre_municipal")]
    public bool? PasseLivreMunicipal { get; set; }

    /// <summary>
    /// Composição familiar
    /// </summary>
    [Column("composicao_familiar")]
    public string? ComposicaoFamiliar { get; set; }

    /// <summary>
    /// Plano de saúde
    /// </summary>
    [Column("plano_saude")]
    public SaudeEnum? PlanoSaude { get; set; }

    /// <summary>
    /// Descrição da gestação
    /// </summary>
    [Column("descricao_gestacao")]
    public string? DescricaoGestacao { get; set; }

    /// <summary>
    /// Uso de medicação pela mãe
    /// </summary>
    [Column("uso_medicacao_mae")]
    public string? UsoMedicacaoMae { get; set; }

    /// <summary>
    /// Semanas de gestação
    /// </summary>
    [Column("gestacao_semanas")]
    public short? GestacaoSemanas { get; set; }

    /// <summary>
    /// Teve internação pós-nascimento
    /// </summary>
    [Column("internacao_pos_nascimento")]
    public bool? InternacaoPosNascimento { get; set; }

    /// <summary>
    /// Médico responsável
    /// </summary>
    [Column("medico_responsavel")]
    public string? MedicoResponsavel { get; set; }

    /// <summary>
    /// Exames realizados
    /// </summary>
    [Column("exames_realizados")]
    public string? ExamesRealizados { get; set; }

    /// <summary>
    /// Doenças físicas
    /// </summary>
    [Column("doencas_fisicas")]
    public string? DoencasFisicas { get; set; }

    /// <summary>
    /// Qualidade do sono
    /// </summary>
    [Column("qualidade_sono")]
    public string? QualidadeSono { get; set; }

    /// <summary>
    /// Cirurgias realizadas
    /// </summary>
    [Column("cirurgias_realizadas")]
    public string? CirurgiasRealizadas { get; set; }

    /// <summary>
    /// Doenças neurológicas
    /// </summary>
    [Column("doencas_neurologicas")]
    public string? DoencasNeurologicas { get; set; }

    /// <summary>
    /// Histórico familiar de doenças
    /// </summary>
    [Column("historico_familiar_doencas")]
    public string? HistoricoFamiliarDoencas { get; set; }

    /// <summary>
    /// Características marcantes
    /// </summary>
    [Column("caracteristicas_marcantes")]
    public string? CaracteristicasMarcantes { get; set; }

    /// <summary>
    /// Boa socialização
    /// </summary>
    [Column("boa_socializacao")]
    public bool? BoaSocializacao { get; set; }

    /// <summary>
    /// Boa adaptação
    /// </summary>
    [Column("boa_adaptacao")]
    public bool? BoaAdaptacao { get; set; }

    /// <summary>
    /// Comportamento agressivo
    /// </summary>
    [Column("comportamento_agressivo")]
    public bool? ComportamentoAgressivo { get; set; }

    /// <summary>
    /// Controle dos esfíncteres
    /// </summary>
    [Column("controle_esfincteres")]
    public bool? ControleEsfincteres { get; set; }

    /// <summary>
    /// Apego familiar
    /// </summary>
    [Column("apego_familiar")]
    public string? ApegoFamiliar { get; set; }

    /// <summary>
    /// Atraso em alimentação
    /// </summary>
    [Column("atraso_alimentacao")]
    public bool? AtrasoAlimentacao { get; set; }

    /// <summary>
    /// Atraso em higiene
    /// </summary>
    [Column("atraso_higiene")]
    public bool? AtrasoHigiene { get; set; }

    /// <summary>
    /// Atraso em vestuário
    /// </summary>
    [Column("atraso_vestuario")]
    public bool? AtrasoVestuario { get; set; }

    /// <summary>
    /// Atraso em locomoção
    /// </summary>
    [Column("atraso_locomocao")]
    public bool? AtrasoLocomocao { get; set; }

    /// <summary>
    /// Atraso em comunicação
    /// </summary>
    [Column("atraso_comunicacao")]
    public bool? AtrasoComunicacao { get; set; }

    /// <summary>
    /// Possui acompanhamento especializado
    /// </summary>
    [Column("acompanhamento_especializado")]
    public bool? AcompanhamentoEspecializado { get; set; }

    /// <summary>
    /// Nome da escola
    /// </summary>
    [Column("nome_escola")]
    public string? NomeEscola { get; set; }

    /// <summary>
    /// Ano escolar
    /// </summary>
    [Column("ano_escola")]
    public string? AnoEscola { get; set; }

    /// <summary>
    /// Turno escolar
    /// </summary>
    [Column("turno_escola")]
    public TurnoEscolaEnum? TurnoEscola { get; set; }

    /// <summary>
    /// Pais casados
    /// </summary>
    [Column("pais_casados")]
    public bool? PaisCasados { get; set; }

    /// <summary>
    /// Paternidade registrada
    /// </summary>
    [Column("paternidade_registrada")]
    public bool? PaternidadeRegistrada { get; set; }

    /// <summary>
    /// Quem é o responsável pela busca
    /// </summary>
    [Column("responsavel_busca")]
    public string? ResponsavelBusca { get; set; }

    /// <summary>
    /// Consentimento de uso de imagem
    /// </summary>
    [Column("consentimento_imagem")]
    public bool? ConsentimentoImagem { get; set; }

    // Navigation properties
    /// <summary>
    /// Convênio associado ao assistido
    /// </summary>
    public Convenio? Convenio { get; set; }

    /// <summary>
    /// Município associado ao assistido
    /// </summary>
    public Municipio? Municipio { get; set; }
}
