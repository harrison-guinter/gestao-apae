namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// DTO de retorno para Município
/// </summary>
public class MunicipioDto
{
    public Guid IdMunicipio { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
}


