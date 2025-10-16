using SistemaApae.Api.Models.Administrative;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Admistrative;

/// <summary>
/// Aplica filtros específicos para consultas de Municipio
/// </summary>
public class MunicipioFilter : IRepositoryFilter<Municipio, MunicipioFilterRequest>
{
    public IPostgrestTable<Municipio> Apply(IPostgrestTable<Municipio> query, MunicipioFilterRequest filtros)
    {
        if (!string.IsNullOrWhiteSpace(filtros.Nome))
        {
            query = query.Filter(a => a.Nome, Supabase.Postgrest.Constants.Operator.ILike, $"%{filtros.Nome}%");
        }

        return query;
    }
}


