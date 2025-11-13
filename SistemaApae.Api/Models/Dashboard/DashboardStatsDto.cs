namespace SistemaApae.Api.Models.Dashboard;

/// <summary>
/// DTO com as estatísticas do dashboard
/// </summary>
public class DashboardStatsDto
{
    /// <summary>
    /// Total de assistidos cadastrados no sistema
    /// </summary>
    public int TotalAssistidos { get; set; }

    /// <summary>
    /// Número de usuários ativos no sistema
    /// </summary>
    public int UsuariosAtivos { get; set; }

    /// <summary>
    /// Número de agendamentos pendentes
    /// </summary>
    public int AgendamentosPendentes { get; set; }

    /// <summary>
    /// Número de atendimentos deste mês
    /// </summary>
    public int AtendimentosMes { get; set; }

    /// <summary>
    /// Número de novos assistidos este mês
    /// </summary>
    public int NovosAssistidosMes { get; set; }

    /// <summary>
    /// Taxa de presença nos atendimentos (%)
    /// </summary>
    public decimal TaxaPresenca { get; set; }
}
