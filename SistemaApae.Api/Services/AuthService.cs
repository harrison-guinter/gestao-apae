using Microsoft.IdentityModel.Tokens;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories;
using SistemaApae.Api.Repositories.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço de autenticação próprio usando banco de dados
/// </summary>
public class AuthService : IAuthService
{
    private readonly IRepository<Usuario, UsuarioFiltroRequest> _repository;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AuthService
    /// </summary>
    /// <param name="repository">Repositório genérico</param>
    /// <param name="emailService">Serviço de email</param>
    /// <param name="configuration">Configuração da aplicação</param>
    /// <param name="logger">Logger para registro de eventos</param>
    public AuthService(
        IRepository<Usuario, UsuarioFiltroRequest> repository,
        IEmailService emailService,
        IConfiguration configuration, 
        ILogger<AuthService> logger)
    {
        _repository = repository;
        _emailService = emailService;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Autentica um usuário com email e senha
    /// </summary>
    /// <param name="request">Dados de login</param>
    /// <returns>Resposta com token JWT e informações do usuário</returns>
    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        try
        {
            // Busca usuário no banco de dados
            var user = (await _repository.GetByFiltersAsync(new UsuarioFiltroRequest { Email = request.Email })).First();

            if (user == null || user.Status == StatusEntidadeEnum.Inativo)
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Credenciais inválidas");
            }

            // Verifica a senha
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Senha))
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Credenciais inválidas");
            }

            // Atualiza último login
            user.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(user);

            // Obtém perfil do usuário
            var perfil = new List<string> { user.Perfil.ToString() };

            // Gera token JWT
            var token = GenerateJwtToken(user.Id.ToString(), user.Email, perfil);

            var loginResponse = new LoginResponse
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = 3600, // 1 hora
                User = new UserInfo
                {
                    Id = user.Id.ToString(),
                    Name = user.Nome,
                    Email = user.Email,
                    Perfil = user.Perfil
                }
            };

            return ApiResponse<LoginResponse>.SuccessResponse(loginResponse, "Login realizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o login para o email: {Email}", request.Email);
            return ApiResponse<LoginResponse>.ErrorResponse("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Gera uma nova senha e envia por email
    /// </summary>
    /// <param name="request">Email para recuperação</param>
    /// <returns>Resposta da operação</returns>
    public async Task<ApiResponse<object>> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        try
        {
            // Busca usuário no banco de dados
            var user = (await _repository.GetByFiltersAsync(new UsuarioFiltroRequest { Email = request.Email })).First();

            if (user != null && user.Status == StatusEntidadeEnum.Ativo)
            {
                // Gera nova senha aleatória
                var newPassword = GenerateRandomPassword();
                
                // Hash da nova senha
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
                
                // Atualiza a senha no banco de dados
                user.Senha = hashedPassword;
                user.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(user);

                // Envia email com a nova senha
                var emailSent = await _emailService.SendEmailAsync(user.Email, user.Nome, newPassword);
            }

            // Por segurança, sempre retorna sucesso
            return ApiResponse<object>.SuccessResponse(
                new { }, 
                "Se o email existir em nosso sistema, você receberá uma nova senha por email."
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar recuperação de senha para: {Email}", request.Email);
            return ApiResponse<object>.ErrorResponse("Erro interno do servidor");
        }
    }

    /// <summary>
    /// Gera um token JWT para o usuário
    /// </summary>
    /// <param name="userId">ID do usuário</param>
    /// <param name="email">Email do usuário</param>
    /// <param name="roles">Roles do usuário</param>
    /// <returns>Token JWT</returns>
    public string GenerateJwtToken(string userId, string email, List<string> roles)
    {
        var jwtKey = _configuration["JWT:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY");
        var jwtIssuer = _configuration["JWT:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER");

        if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
        {
            throw new InvalidOperationException("Configurações JWT não encontradas. Verifique JWT_KEY e JWT_ISSUER.");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.Name, email),
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Adiciona roles como claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Gera uma senha aleatória segura
    /// </summary>
    /// <returns>Senha aleatória de 12 caracteres</returns>
    public string GenerateRandomPassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
