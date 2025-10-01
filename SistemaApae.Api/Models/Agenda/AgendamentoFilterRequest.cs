using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// Modelo para requisição de Municipio por filtros de pesquisa
/// </summary>
public class AgendamentoFilterRequest : BaseFilter
{
    /// <summary>
    /// Data inicial do agendamento 
    /// </summary>
    public DateOnly? DataAgendamentoInicio { get; set; }

    /// <summary>
    /// Data final do agendamento 
    /// </summary>
    public DateOnly? DataAgendamentoFim { get; set; }

    /// <summary>
    /// ID do assistido
    /// </summary>
    public Guid IdAssistido { get; set; }

    /// <summary>
    /// ID do profissional responsável
    /// </summary>
    public Guid IdProfissional { get; set; }

}

