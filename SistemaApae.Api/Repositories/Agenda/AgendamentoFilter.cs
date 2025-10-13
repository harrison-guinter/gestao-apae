using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest;

public class AgendamentoFilter : IRepositoryFilter<Agendamento, AgendamentoFilterRequest>
{
    public IPostgrestTable<Agendamento> Apply(IPostgrestTable<Agendamento> query, AgendamentoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Intervalo de datas
        if (filtros.DataAgendamentoInicio.HasValue && filtros.DataAgendamentoFim.HasValue)
        {
            query = query.Filter(a => a.DataAgendamento, Constants.Operator.GreaterThanOrEqual, filtros.DataAgendamentoInicio.Value)
                         .Filter(a => a.DataAgendamento, Constants.Operator.LessThanOrEqual, filtros.DataAgendamentoFim.Value);
        }
        else if (filtros.DataAgendamentoInicio.HasValue)
        {
            query = query.Filter(a => a.DataAgendamento, Constants.Operator.GreaterThanOrEqual, filtros.DataAgendamentoInicio.Value);
        }
        else if (filtros.DataAgendamentoFim.HasValue)
        {
            query = query.Filter(a => a.DataAgendamento, Constants.Operator.LessThanOrEqual, filtros.DataAgendamentoFim.Value);
        }

        // Filtro por profissional
        if (filtros.Profissional.Id != Guid.Empty)
        {
            query = query.Filter(a => a.IdProfissional, Constants.Operator.Equals, filtros.Profissional.Id);
        }

        return query;
    }
}
