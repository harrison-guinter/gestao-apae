using SistemaApae.Api.Models.Agenda;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Admistrative;

/// <summary>
/// Aplica filtros específicos para consultas de Municipio
/// </summary>
public class AgendamentoFilter : IRepositoryFilter<Agendamento, AgendamentoFilterRequest>
{
    public IPostgrestTable<Agendamento> Apply(IPostgrestTable<Agendamento> query, AgendamentoFilterRequest filtros)
    {
        return query;
    }
}


