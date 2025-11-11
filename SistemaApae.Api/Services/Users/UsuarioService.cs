using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories;

namespace SistemaApae.Api.Services.Users;

public class UsuarioService : Service<Usuario, UsuarioFilterRequest>
{
    private readonly ILogger<UsuarioService> _logger;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;

    /// <summary>
    /// Inicializa uma nova instância do UsuarioService
    /// </summary>
    public UsuarioService(
        IRepository<Usuario, UsuarioFilterRequest> repository,
        ILogger<UsuarioService> logger,
        IAuthService authService, 
        IEmailService emailService
    ) : base(repository, logger)
    {
        _logger = logger;
        _authService = authService;
        _emailService = emailService;
    }

    /// <summary>
    /// Lista usuários por filtros de pesquisa
    /// </summary>
    public async Task<ApiResponse<IEnumerable<UsuarioDto>>> GetByFilters(UsuarioFilterRequest filters)
    {
        try
        {
            var result = await base.GetByFilters(filters);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Registros não foram encontrados");
            }

            var response = new List<UsuarioDto>();

            foreach (var usuario in result.Data)
            {
                response.Add(new UsuarioDto
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    Telefone = usuario.Telefone,
                    Perfil = usuario.Perfil,
                    RegistroProfissional = usuario.RegistroProfissional,
                    Especialidade = usuario.Especialidade,
                    Observacao = usuario.Observacao,
                    Status = usuario.Status
                });
            }

            return ApiResponse<IEnumerable<UsuarioDto>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar usuários por filtros");
            return ApiResponse<IEnumerable<UsuarioDto>>.ErrorResponse("Erro interno ao buscar usuários por filtros");
        }
    }

    /// <summary>
    /// Buscar um usuário por id
    /// </summary>
    public async Task<ApiResponse<UsuarioDto>> GetById(Guid id)
    {
        try
        {
            var result = await base.GetById(id);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<UsuarioDto>.ErrorResponse("Registro não foi encontrado");
            }

            var response = new UsuarioDto
            {
                Id = result.Data.Id,
                Nome = result.Data.Nome,
                Email = result.Data.Email,
                Telefone = result.Data.Telefone,
                Perfil = result.Data.Perfil,
                RegistroProfissional = result.Data.RegistroProfissional,
                Especialidade = result.Data.Especialidade,
                Observacao = result.Data.Observacao,
                Status = result.Data.Status
            };

            return ApiResponse<UsuarioDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar usuário por id");
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao buscar usuário por id");
        }
    }

    /// <summary>
    /// Criar um usuário
    /// </summary>
    public async Task<ApiResponse<UsuarioDto>> Create(Usuario user)
    {
        try
        {
            var newPassword = _authService.GenerateRandomPassword();

            user.UpdatedAt = DateTime.UtcNow;
            user.Senha = BCrypt.Net.BCrypt.HashPassword(newPassword);

            var result = await base.Create(user);

            if (!result.Success || result.Data == null)
            {
                if (result.Message.Contains("Erro de duplicidade de registro"))
                    return ApiResponse<UsuarioDto>.ErrorResponse("Erro de duplicidade de registro");

                return ApiResponse<UsuarioDto>.ErrorResponse("Registro não foi adicionado");
            }

            var response = new UsuarioDto
            {
                Id = result.Data.Id,
                Nome = result.Data.Nome,
                Email = result.Data.Email,
                Telefone = result.Data.Telefone,
                Perfil = result.Data.Perfil,
                RegistroProfissional = result.Data.RegistroProfissional,
                Especialidade = result.Data.Especialidade,
                Observacao = result.Data.Observacao,
                Status = result.Data.Status
            };

            await _emailService.SendEmailAsync(response.Email, response.Nome, newPassword, EmailReasonEnum.CreateUser);

            return ApiResponse<UsuarioDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao adicionar usuário");
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao adicionar usuário");
        }
    }

    /// <summary>
    /// Atualiza um usuário existente
    /// </summary>
    public async Task<ApiResponse<UsuarioDto>> Update(Usuario user)
    {
        try
        {
            user.Senha = (await base.GetById(user.Id)).Data!.Senha;

            var result = await base.Update(user);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<UsuarioDto>.ErrorResponse("Registro não foi atualizado");
            }

            var response = new UsuarioDto
            {
                Id = result.Data.Id,
                Nome = result.Data.Nome,
                Email = result.Data.Email,
                Telefone = result.Data.Telefone,
                Perfil = result.Data.Perfil,
                RegistroProfissional = result.Data.RegistroProfissional,
                Especialidade = result.Data.Especialidade,
                Observacao = result.Data.Observacao,
                Status = result.Data.Status
            };

            return ApiResponse<UsuarioDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao atualizar usuário");
            return ApiResponse<UsuarioDto>.ErrorResponse("Erro interno ao atualizar usuário");
        }
    }
}
