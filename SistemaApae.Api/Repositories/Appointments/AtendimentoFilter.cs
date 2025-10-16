using SistemaApae.Api.Models.Appointment;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Appointments;

/// <summary>
/// Aplica filtros específicos para consultas de Atendimento
/// </summary>
public class AtendimentoFilter : IRepositoryFilter<Atendimento, AtendimentoFilterRequest>
{
    public IPostgrestTable<Atendimento> Apply(IPostgrestTable<Atendimento> query, AtendimentoFilterRequest filtros)
    {
        if (filtros.IdAgendamento != Guid.Empty)
            query = query.Filter(a => a.IdAgendamento, Constants.Operator.Equals, filtros.IdAgendamento);

        if (filtros.IdAssistido != Guid.Empty)
            query = query.Filter(a => a.IdAgendamento, Constants.Operator.Equals, filtros.IdAssistido);

        if (filtros.DataInicioAtendimento.HasValue && filtros.DataFimAtendimento.HasValue)
        {
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, filtros.DataInicioAtendimento.Value)
                         .Filter(a => a.DataAtendimento!, Constants.Operator.LessThanOrEqual, filtros.DataFimAtendimento.Value);
        }
        else if (filtros.DataInicioAtendimento.HasValue)
        {
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, filtros.DataInicioAtendimento.Value);
        }
        else if (filtros.DataFimAtendimento.HasValue)
        {
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.LessThanOrEqual, filtros.DataFimAtendimento.Value);
        }

        if (filtros.Presenca != null)
            query = query.Filter(a => a.Status!, Constants.Operator.Equals, (int)filtros.Presenca);

        return query;
    }
}