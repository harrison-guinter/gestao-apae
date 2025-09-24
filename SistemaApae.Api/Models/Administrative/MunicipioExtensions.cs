namespace SistemaApae.Api.Models.Administrative;

/// <summary>
/// Extensões de mapeamento para Município
/// </summary>
public static class MunicipioExtensions
{
    public static MunicipioDto ToDto(this Municipio municipio)
    {
        return new MunicipioDto
        {
            IdMunicipio = municipio.IdMunicipio,
            Nome = municipio.Nome,
            Uf = municipio.Uf
        };
    }

    public static IEnumerable<MunicipioDto> ToDto(this IEnumerable<Municipio> municipios)
    {
        return municipios.Select(m => m.ToDto());
    }
}


