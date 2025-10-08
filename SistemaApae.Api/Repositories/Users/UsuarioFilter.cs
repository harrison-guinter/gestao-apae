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
            query = query.Where(u => u.Perfil == filtros.Perfil);

        //query = query.Where(u => u.Status == filtros.Status);

        return query;
    }
}
