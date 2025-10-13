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
    /// Assistido
    /// </summary>
    public AssistidoDto Assistido { get; set; }

    /// <summary>
    /// Profissional responsável
    /// </summary>
    public UsuarioDto Profissional { get; set; }

}

