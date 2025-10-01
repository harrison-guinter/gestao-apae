using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo para requisição de Municipio por filtros de pesquisa
/// </summary>
public class AgendamentoFilterRequest : BaseFilter
{
    /// <summary>
    /// Data do agendamento
    /// </summary>
    public DateOnly? DataAgendamento { get; set; }

    /// <summary>
    /// ID do assistido
    /// </summary>
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// ID do profissional responsável
    /// </summary>
    public Guid IdProfissional { get; set; }

}

