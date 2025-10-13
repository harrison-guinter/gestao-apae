using SistemaApae.Api.Models.Patients;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Patients;

/// <summary>
/// Aplica filtros específicos para consultas de Assistido
/// </summary>
public class AssistidoFilter : IRepositoryFilter<Assistido, AssistidoFiltroRequest>
{
    public IPostgrestTable<Assistido> Apply(IPostgrestTable<Assistido> query, AssistidoFiltroRequest filtros)
    {
        if (!string.IsNullOrWhiteSpace(filtros.Nome))
        {
            query = query.Filter(a => a.Nome, Constants.Operator.ILike, $"%{filtros.Nome}%");
        }

        if (!string.IsNullOrWhiteSpace(filtros.CPF))
        {
            // Busca parcial por CPF (normaliza para conter somente dígitos)
            var digits = new string(filtros.CPF.Where(char.IsDigit).ToArray());
            if (!string.IsNullOrEmpty(digits))
            {
                query = query.Filter(a => a.Cpf!, Constants.Operator.ILike, $"%{digits}%");
            }
        }

        query = query.Filter(u => u.Status, Constants.Operator.Equals, (int)filtros.Status);

        return query;
    }
}


