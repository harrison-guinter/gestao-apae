using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Dashboard;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Services;

namespace SistemaApae.Api.Services;

/// <summary>
/// Serviço de dashboard
/// </summary>
public class DashboardService
{
    private readonly ISupabaseService _supabaseService;
    private readonly ILogger<DashboardService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do serviço de dashboard
    /// </summary>
    public DashboardService(ISupabaseService supabaseService, ILogger<DashboardService> logger)
    {
        _supabaseService = supabaseService;
        _logger = logger;
    }

    /// <summary>
    /// Obtém as estatísticas do dashboard
    /// </summary>
    /// <returns>Resposta com as estatísticas do dashboard</returns>
    public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStats()
    {
        try
        {
            var hoje = DateOnly.FromDateTime(DateTime.Now);
            var inicioSemana = hoje.AddDays(-(int)DateTime.Now.DayOfWeek);
            var inicioMes = new DateOnly(hoje.Year, hoje.Month, 1);

            // Buscar total de assistidos
            var totalAssistidos = await _supabaseService.Client
                .From<Models.Patients.Assistido>()
                .Where(a => a.Status == StatusEntidadeEnum.Ativo)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar atendimentos de hoje
            var atendimentosHoje = await _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Where(a => a.DataAtendimento.HasValue && 
                           DateOnly.FromDateTime(a.DataAtendimento.Value) == hoje)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar usuários ativos
            var usuariosAtivos = await _supabaseService.Client
                .From<Models.Users.Usuario>()
                .Where(u => u.Status == StatusEntidadeEnum.Ativo)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar agendamentos pendentes (agendamentos futuros sem atendimento)
            var agendamentosPendentes = await _supabaseService.Client
                .From<Models.Agenda.Agendamento>()
                .Where(a => a.Status == StatusEntidadeEnum.Ativo &&
                           a.DataAgendamento.HasValue &&
                           a.DataAgendamento.Value >= hoje)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar atendimentos da semana
            var atendimentosSemana = await _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Where(a => a.DataAtendimento.HasValue &&
                           DateOnly.FromDateTime(a.DataAtendimento.Value) >= inicioSemana &&
                           DateOnly.FromDateTime(a.DataAtendimento.Value) <= hoje)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar atendimentos do mês
            var atendimentosMes = await _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Where(a => a.DataAtendimento.HasValue &&
                           DateOnly.FromDateTime(a.DataAtendimento.Value) >= inicioMes &&
                           DateOnly.FromDateTime(a.DataAtendimento.Value) <= hoje)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Buscar novos assistidos do mês
            var novosAssistidosMes = await _supabaseService.Client
                .From<Models.Patients.Assistido>()
                .Where(a => a.Status == StatusEntidadeEnum.Ativo &&
                           a.DataCadastro.HasValue &&
                           a.DataCadastro.Value >= inicioMes)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            // Calcular taxa de presença (atendimentos com presença confirmada vs total de atendimentos)
            var atendimentosComPresenca = await _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Where(a => a.Presenca == StatusAtendimentoEnum.PRESENCA)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            var totalAtendimentos = await _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Where(a => a.Presenca.HasValue)
                .Count(Supabase.Postgrest.Constants.CountType.Exact);

            var taxaPresenca = totalAtendimentos > 0 
                ? Math.Round((decimal)atendimentosComPresenca / totalAtendimentos * 100, 2)
                : 0;

            var stats = new DashboardStatsDto
            {
                TotalAssistidos = totalAssistidos,
                AtendimentosHoje = atendimentosHoje,
                UsuariosAtivos = usuariosAtivos,
                AgendamentosPendentes = agendamentosPendentes,
                AtendimentosSemana = atendimentosSemana,
                AtendimentosMes = atendimentosMes,
                NovosAssistidosMes = novosAssistidosMes,
                TaxaPresenca = taxaPresenca
            };

            return ApiResponse<DashboardStatsDto>.SuccessResponse(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar estatísticas do dashboard");
            return ApiResponse<DashboardStatsDto>.ErrorResponse("Erro interno ao buscar estatísticas do dashboard");
        }
    }
}
