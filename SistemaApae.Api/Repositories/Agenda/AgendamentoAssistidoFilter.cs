using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest;

namespace SistemaApae.Api.Repositories.Agenda;

public class AgendamentoAssistidoFilter : IRepositoryFilter<AgendamentoAssistido, AgendamentoAssistidoFilterRequest>
{
    public IPostgrestTable<AgendamentoAssistido> Apply(IPostgrestTable<AgendamentoAssistido> query, AgendamentoAssistidoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Filtro por agendamento
        if (filtros.IdAgendamento != Guid.Empty)
        {
            query = query.Filter(ga => ga.IdAgendamento, Constants.Operator.Equals, filtros.IdAgendamento);
        }

        // Filtro por assistido
        if (filtros.IdAssistido != Guid.Empty)
        {
            query = query.Filter(ga => ga.IdAssistido, Constants.Operator.Equals, filtros.IdAssistido);
        }

        // Filtro por status
        if (filtros.Status.HasValue)
        {
            query = query.Filter(ga => ga.Status, Constants.Operator.Equals, filtros.Status.Value);
        }

        return query;
    }
}
