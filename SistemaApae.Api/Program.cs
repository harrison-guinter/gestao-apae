using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SistemaApae.Api.Repositories.Users;
using SistemaApae.Api.Repositories.Administrative;
using SistemaApae.Api.Services;
using SistemaApae.Api.Services.Users;
using SistemaApae.Api.Services.Administrative;
using System.Text;
using System.Text.Json.Serialization;
using SistemaApae.Api.Serialization;
using SistemaApae.Api.Repositories.Agreements;
using SistemaApae.Api.Services.Agreements;

var builder = WebApplication.CreateBuilder(args);

// Configurar para usar a porta do Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Registrar Supabase
builder.Services.AddSingleton<ISupabaseService, SupabaseService>();

// Configurar Autenticação JWT
var jwtKey = builder.Configuration["JWT:Key"] ?? Environment.GetEnvironmentVariable("JWT_KEY");
var jwtIssuer = builder.Configuration["JWT:Issuer"] ?? Environment.GetEnvironmentVariable("JWT_ISSUER");

if (!string.IsNullOrEmpty(jwtKey) && !string.IsNullOrEmpty(jwtIssuer))
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtIssuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.Zero
            };
        });
}

// Registrar repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IMunicipioRepository, MunicipioRepository>();
builder.Services.AddScoped<IConvenioRepository, ConvenioRepository>();

// Registrar serviços
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IMunicipioService, MunicipioService>();
builder.Services.AddScoped<IConvenioService, ConvenioService>();

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = null;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        opt.JsonSerializerOptions.TypeInfoResolverChain.Insert(0, new SupabaseModelJsonResolver());
    });

builder.Services.AddEndpointsApiExplorer();

// Configurar CORS para produção
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Sistema APAE API",
        Version = "v1.0.0",
        Description = "API para gerenciamento do Sistema APAE"
    });

    // Incluir comentários XML na documentação
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Adiciona o SchemaFilter para remover propriedades internas do BaseModel
    c.SchemaFilter<RemoveSupabaseBasePropertiesFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema APAE API v1.0.0");
    c.RoutePrefix = "docs"; // Acessível em /docs
});

// Usar CORS
app.UseCors();

// Usar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
