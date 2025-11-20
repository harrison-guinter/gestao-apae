namespace SistemaApae.Api.Models.Reports.Presencas;

/// <summary>
/// Linha agregada do relatório de presença (entrega para municípios)
/// </summary>
public class PresencaListaItemDto
{
	public int Numero { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string? Endereco { get; set; }
	public string? TipoAtendimento { get; set; }
	public string? DiaTerapias { get; set; }
	public string? DiaSemana { get; set; }
	public string? Turno { get; set; }
}


