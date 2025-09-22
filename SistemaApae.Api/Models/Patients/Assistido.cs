using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;
using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo de assistido do sistema
/// </summary>
[Table("assistido")]
public class Assistido : BaseModel
{
    /// <summary>
    /// ID único do assistido
    /// </summary>
    [Column("id_assistido")]
    public Guid IdAssistido { get; set; }

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
    /// Indica se o assistido está ativo
    /// </summary>
    [Column("ativo")]
    public bool? Ativo { get; set; }

    /// <summary>
    /// Data de cadastro do assistido
    /// </summary>
    [Column("data_cadastro")]
    public DateOnly? DataCadastro { get; set; }

    /// <summary>
    /// Situação do assistido
    /// </summary>
    [Column("situacao")]
    public SituacaoEnum? Situacao { get; set; }

    /// <summary>
    /// Área de atendimento do assistido
    /// </summary>
    [Column("area_atendimento")]
    public AreaAtendimentoEnum? AreaAtendimento { get; set; }

    /// <summary>
    /// RG do assistido
    /// </summary>
    [Column("rg")]
    public string? Rg { get; set; }

    /// <summary>
    /// Data de emissão do RG
    /// </summary>
    [Column("data_emissao_rg")]
    public DateOnly? DataEmissaoRg { get; set; }

    /// <summary>
    /// Número da certidão
    /// </summary>
    [Column("certidao_numero")]
    public string? CertidaoNumero { get; set; }

    /// <summary>
    /// Livro e folha da certidão
    /// </summary>
    [Column("certidao_livro_folha")]
    public string? CertidaoLivroFolha { get; set; }

    /// <summary>
    /// Cartório da certidão
    /// </summary>
    [Column("certidao_cartorio")]
    public string? CertidaoCartorio { get; set; }

    /// <summary>
    /// Naturalidade do assistido
    /// </summary>
    [Column("naturalidade")]
    public string? Naturalidade { get; set; }

    /// <summary>
    /// Sexo do assistido
    /// </summary>
    [Column("sexo")]
    public SexoEnum? Sexo { get; set; }

    /// <summary>
    /// Número da carteira PCD
    /// </summary>
    [Column("carteira_pcd")]
    public string? CarteiraPcd { get; set; }

    /// <summary>
    /// Número do NIS
    /// </summary>
    [Column("nis")]
    public string? Nis { get; set; }

    /// <summary>
    /// Tipo de deficiência
    /// </summary>
    [Column("tipo_deficiencia")]
    public TipoDeficienciaEnum? TipoDeficiencia { get; set; }

    /// <summary>
    /// CID (Classificação Internacional de Doenças)
    /// </summary>
    [Column("cid")]
    public string? Cid { get; set; }

    /// <summary>
    /// CBDF
    /// </summary>
    [Column("cbdf")]
    public string? Cbdf { get; set; }

    /// <summary>
    /// Informações sobre mobilidade
    /// </summary>
    [Column("mobilidade")]
    public string? Mobilidade { get; set; }

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
    /// Alergias do assistido
    /// </summary>
    [Column("alergias")]
    public string? Alergias { get; set; }

    /// <summary>
    /// Comorbidades do assistido
    /// </summary>
    [Column("comorbidades")]
    public string? Comorbidades { get; set; }

    /// <summary>
    /// Indica se está liberado para atividade física
    /// </summary>
    [Column("liberado_atividade_fisica")]
    public bool? LiberadoAtividadeFisica { get; set; }

    /// <summary>
    /// Nome da mãe
    /// </summary>
    [Column("nome_mae")]
    public string? NomeMae { get; set; }

    /// <summary>
    /// CPF da mãe
    /// </summary>
    [Column("cpf_mae")]
    public string? CpfMae { get; set; }

    /// <summary>
    /// Telefone da mãe
    /// </summary>
    [Column("telefone_mae")]
    public string? TelefoneMae { get; set; }

    /// <summary>
    /// Email da mãe
    /// </summary>
    [Column("email_mae")]
    public string? EmailMae { get; set; }

    /// <summary>
    /// Nome do pai
    /// </summary>
    [Column("nome_pai")]
    public string? NomePai { get; set; }

    /// <summary>
    /// CPF do pai
    /// </summary>
    [Column("cpf_pai")]
    public string? CpfPai { get; set; }

    /// <summary>
    /// Telefone do pai
    /// </summary>
    [Column("telefone_pai")]
    public string? TelefonePai { get; set; }

    /// <summary>
    /// Email do pai
    /// </summary>
    [Column("email_pai")]
    public string? EmailPai { get; set; }

    /// <summary>
    /// Nome do responsável legal
    /// </summary>
    [Column("responsavel_legal_nome")]
    public string? ResponsavelLegalNome { get; set; }

    /// <summary>
    /// CPF do responsável legal
    /// </summary>
    [Column("responsavel_legal_cpf")]
    public string? ResponsavelLegalCpf { get; set; }

    /// <summary>
    /// Telefone do responsável legal
    /// </summary>
    [Column("responsavel_legal_telefone")]
    public string? ResponsavelLegalTelefone { get; set; }

    /// <summary>
    /// Email do responsável legal
    /// </summary>
    [Column("responsavel_legal_email")]
    public string? ResponsavelLegalEmail { get; set; }

    /// <summary>
    /// Indica se deseja receber notificações
    /// </summary>
    [Column("receber_notificacoes")]
    public bool ReceberNotificacoes { get; set; } = false;

    /// <summary>
    /// Autorização para uso de imagem
    /// </summary>
    [Column("autorizacao_imagem")]
    public bool AutorizacaoImagem { get; set; } = false;

    /// <summary>
    /// CPF do assistido
    /// </summary>
    [Column("cpf")]
    public string? Cpf { get; set; }

    // Navigation properties
    /// <summary>
    /// Convênio CAS associado ao assistido
    /// </summary>
    public ConvenioCas? ConvenioCas { get; set; }
}
