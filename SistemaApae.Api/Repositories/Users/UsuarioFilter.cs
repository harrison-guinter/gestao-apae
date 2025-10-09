using SistemaApae.Api.Models.Users;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Users;

/// <summary>
/// Aplica filtros específicos para consultas de Usuario
/// </summary>
public class UsuarioFilter : IRepositoryFilter<Usuario, UsuarioFiltroRequest>
{
    public IPostgrestTable<Usuario> Apply(IPostgrestTable<Usuario> query, UsuarioFiltroRequest filtros)
    {
        if (!string.IsNullOrEmpty(filtros.Email))
            query = query.Filter(u => u.Email, Constants.Operator.ILike, $"%{filtros.Email}%");

        if (!string.IsNullOrEmpty(filtros.Nome))
            query = query.Filter(u => u.Nome, Constants.Operator.ILike, $"%{filtros.Nome}%");

        if (filtros.Perfil != null)
            query = query.Filter(u => u.Perfil, Constants.Operator.Equals, (int)filtros.Perfil);

        query = query.Filter(u => u.Status, Constants.Operator.Equals, (int)filtros.Status);

        return query;
    }
}
