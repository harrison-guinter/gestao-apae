using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Users;

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
    /// ID do assistido para filtrar
    /// </summary>
    public Guid IdAssistido { get; set; } = Guid.Empty;

    /// <summary>
    /// ID do Profissional para filtrar
    /// </summary>
    public Guid IdProfissional { get; set; } = Guid.Empty;

    /// <summary>
    /// Status do Agendamento (ativo/inativo)
    /// </summary>
    public StatusEntidadeEnum Status { get; set; } = StatusEntidadeEnum.Ativo;

}

