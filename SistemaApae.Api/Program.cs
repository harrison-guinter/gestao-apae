var builder = WebApplication.CreateBuilder(args);

// Configurar para usar a porta do Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// Registrar Supabase
builder.Services.AddSingleton<ISupabaseService, SupabaseService>();

builder.Services.AddControllers();
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
});

var app = builder.Build();

// Configurar Swagger apenas em Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sistema APAE API v1.0.0");
        c.RoutePrefix = "docs"; // Acessível em /docs
    });
}

// Usar CORS
app.UseCors();

// Remover HTTPS redirect para Render (usa proxy reverso)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();
