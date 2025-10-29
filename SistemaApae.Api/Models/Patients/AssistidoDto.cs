using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Patients;

public class AssistidoDto
{

    /// <summary>
    /// ID único da entidade
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Nome do assistido
    /// </summary>
    public string? Nome { get; set; } = string.Empty;

    /// <summary>
    /// Indica se a entidade está ativa/inativa
    /// </summary>
    public StatusEntidadeEnum? Status { get; set; }

    /// <summary>
    /// Nome do convênio do assistido
    /// </summary>
    public string? NomeConvenio { get; set; }

    /// <summary>
    /// Nome da Cidade do assistido
    /// </summary>
    public string? NomeCidade { get; set; }
}

