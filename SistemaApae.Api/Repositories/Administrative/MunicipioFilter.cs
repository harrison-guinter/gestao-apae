using SistemaApae.Api.Models.Administrative;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Admistrative;

/// <summary>
/// Aplica filtros específicos para consultas de Municipio
/// </summary>
public class MunicipioFilter : IRepositoryFilter<Municipio, MunicipioFiltroRequest>
{
    public IPostgrestTable<Municipio> Apply(IPostgrestTable<Municipio> query, MunicipioFiltroRequest filtros)
    {
        if (!string.IsNullOrWhiteSpace(filtros.Nome))
        {
            query = query.Filter(a => a.Nome, Supabase.Postgrest.Constants.Operator.ILike, $"%{filtros.Nome}%");
        }

        return query;
    }
}


