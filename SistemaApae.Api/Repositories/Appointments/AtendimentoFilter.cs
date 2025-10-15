using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Appointments;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Appointments;

/// <summary>
/// Aplica filtros específicos para consultas de Atendimento
/// </summary>
public class AtendimentoFilter : IRepositoryFilter<Atendimento, AtendimentoFiltroRequest>
{
    public IPostgrestTable<Atendimento> Apply(IPostgrestTable<Atendimento> query, AtendimentoFiltroRequest filtros)
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

        if (filtros.Status != null)
            query = query.Filter(a => a.Status!, Constants.Operator.Equals, (int)filtros.Status);

        return query;
    }
}