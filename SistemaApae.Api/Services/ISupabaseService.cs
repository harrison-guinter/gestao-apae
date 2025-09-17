using Supabase;

namespace SistemaApae.Api.Services;

/// <summary>
/// Interface para o serviço do Supabase
/// </summary>
public interface ISupabaseService
{
    /// <summary>
    /// Cliente Supabase configurado
    /// </summary>
    Supabase.Client Client { get; }

    /// <summary>
    /// Verifica se a conexão com o Supabase está funcionando
    /// </summary>
    /// <returns>True se a conexão estiver funcionando</returns>
    Task<bool> IsConnectedAsync();
}
