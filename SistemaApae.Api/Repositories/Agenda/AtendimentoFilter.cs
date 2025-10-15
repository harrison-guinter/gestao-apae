using SistemaApae.Api.Models.Agenda.Atendimento;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest;

namespace SistemaApae.Api.Repositories.Agenda;

/// <summary>
/// Filtro para atendimentos
/// </summary>
public class AtendimentoFilter : IRepositoryFilter<Atendimento, AtendimentoFilterRequest>
{
    /// <summary>
    /// Aplica filtros de busca à query de atendimentos
    /// </summary>
    public IPostgrestTable<Atendimento> Apply(IPostgrestTable<Atendimento> query, AtendimentoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Filtro por agendamento
        if (filtros.IdAgendamento != Guid.Empty)
        {
            query = query.Filter(a => a.IdAgendamento, Constants.Operator.Equals, filtros.IdAgendamento);
        }

        // Filtro por assistido
        if (filtros.IdAssistido != Guid.Empty)
        {
            query = query.Filter(a => a.IdAssistido, Constants.Operator.Equals, filtros.IdAssistido);
        }

        // Filtro por data inicial
        if (filtros.DataAtendimentoInicio.HasValue)
        {
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, filtros.DataAtendimentoInicio.Value);
        }

        // Filtro por data final
        if (filtros.DataAtendimentoFim.HasValue)
        {
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.LessThanOrEqual, filtros.DataAtendimentoFim.Value);
        }

        // Filtro por presença (status do atendimento)
        if (filtros.Presenca.HasValue)
        {
            query = query.Filter(a => a.Presenca!, Constants.Operator.Equals, filtros.Presenca.Value);
        }

        return query;
    }
}

