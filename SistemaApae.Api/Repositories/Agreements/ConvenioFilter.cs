using SistemaApae.Api.Models.Agreements;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

public class ConvenioFilter : IRepositoryFilter<Convenio, ConvenioFilterRequest>
{
    public IPostgrestTable<Convenio> Apply(IPostgrestTable<Convenio> query, ConvenioFilterRequest filters)
    {
        if (filters == null)
            return query;

        if (!string.IsNullOrEmpty(filters.Nome))
            query = query.Filter(u => u.Nome, Constants.Operator.ILike, $"%{filters.Nome}%");

        if (filters.IdMunicipio != Guid.Empty)
        {
            query = query.Filter(u => u.IdMunicipio, Constants.Operator.Equals, filters.IdMunicipio);
        }

        return query;
    }
}
