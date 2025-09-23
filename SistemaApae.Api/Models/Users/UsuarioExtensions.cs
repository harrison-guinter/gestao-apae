namespace SistemaApae.Api.Models.Users;

/// <summary>
/// Extensões para conversão entre Usuario e UsuarioDto
/// </summary>
public static class UsuarioExtensions
{
    /// <summary>
    /// Converte Usuario para UsuarioDto
    /// </summary>
    /// <param name="usuario">Modelo Usuario</param>
    /// <returns>DTO UsuarioDto</returns>
    public static UsuarioDto ToDto(this Usuario usuario)
    {
        return new UsuarioDto
        {
            IdUsuario = usuario.IdUsuario,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Telefone = usuario.Telefone,
            Perfil = usuario.Perfil,
            Status = usuario.Status,
            Observacao = usuario.Observacao,
            RegistroProfissional = usuario.RegistroProfissional,
            Especialidade = usuario.Especialidade
        };
    }

    /// <summary>
    /// Converte lista de Usuario para lista de UsuarioDto
    /// </summary>
    /// <param name="usuarios">Lista de Usuario</param>
    /// <returns>Lista de UsuarioDto</returns>
    public static IEnumerable<UsuarioDto> ToDto(this IEnumerable<Usuario> usuarios)
    {
        return usuarios.Select(u => u.ToDto());
    }
}
