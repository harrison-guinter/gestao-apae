using SistemaApae.Api.Models.Agenda;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Agenda;

public class AgendamentoAssistidoFilter : IRepositoryFilter<AgendamentoAssistido, AgendamentoAssistidoFilterRequest>
{
    public IPostgrestTable<AgendamentoAssistido> Apply(IPostgrestTable<AgendamentoAssistido> query, AgendamentoAssistidoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Filtro por lista de agendamentos (busca em lote)
        if (filtros.IdsAgendamento != null && filtros.IdsAgendamento.Count > 0)
        {
            query = query.Filter(ga => ga.IdAgendamento, Constants.Operator.In, filtros.IdsAgendamento);
        }

        // Filtro por agendamento
        if (filtros.IdAgendamento != Guid.Empty)
        {
            query = query.Where(ga => ga.IdAgendamento == filtros.IdAgendamento);
        }

        // Filtro por assistido
        if (filtros.IdAssistido != Guid.Empty)
        {
            query = query.Where(ga => ga.IdAssistido == filtros.IdAssistido);
        }

        // Filtro por status
        if (filtros.Status.HasValue)
        {
            query = query.Filter(u => u.Status, Constants.Operator.Equals, (int)filtros.Status);
        }

        return query;
    }
}
