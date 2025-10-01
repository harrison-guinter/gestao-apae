using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Interfaces;

public class AgendamentoFilter : IRepositoryFilter<Agendamento, AgendamentoFilterRequest>
{
    public IPostgrestTable<Agendamento> Apply(IPostgrestTable<Agendamento> query, AgendamentoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Intervalo de datas
        if (filtros.DataAgendamentoInicio.HasValue && filtros.DataAgendamentoFim.HasValue)
        {
            query = query.Filter("data_agendamento", Supabase.Postgrest.Constants.Operator.GreaterThanOrEqual, filtros.DataAgendamentoInicio.Value)
                         .Filter("data_agendamento", Supabase.Postgrest.Constants.Operator.LessThanOrEqual, filtros.DataAgendamentoFim.Value);
        }
        else if (filtros.DataAgendamentoInicio.HasValue)
        {
            query = query.Filter("data_agendamento", Supabase.Postgrest.Constants.Operator.GreaterThanOrEqual, filtros.DataAgendamentoInicio.Value);
        }
        else if (filtros.DataAgendamentoFim.HasValue)
        {
            query = query.Filter("data_agendamento", Supabase.Postgrest.Constants.Operator.LessThanOrEqual, filtros.DataAgendamentoFim.Value);
        }

        // Filtro por profissional
        if (filtros.IdProfissional != Guid.Empty)
        {
            query = query.Filter("id_profissional", Supabase.Postgrest.Constants.Operator.Equals, filtros.IdProfissional);
        }

        return query;
    }
}
