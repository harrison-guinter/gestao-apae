using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Dashboard;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Services;
using Supabase.Postgrest;

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

            // Faixas de tempo em DateTime para facilitar filtros (evita métodos dentro do Where)
            var inicioHoje = hoje.ToDateTime(TimeOnly.MinValue);
            var inicioAmanha = hoje.AddDays(1).ToDateTime(TimeOnly.MinValue);
            var inicioSemanaDateTime = inicioSemana.ToDateTime(TimeOnly.MinValue);
            var inicioMesDateTime = inicioMes.ToDateTime(TimeOnly.MinValue);

            // Converte datas para strings compatíveis com PostgREST
            string inicioHojeStr = inicioHoje.ToString("yyyy-MM-dd HH:mm:ss");
            string inicioAmanhaStr = inicioAmanha.ToString("yyyy-MM-dd HH:mm:ss");
            string inicioSemanaStr = inicioSemanaDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string inicioMesStr = inicioMesDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string hojeDateStr = hoje.ToString("yyyy-MM-dd");
            string inicioMesDateStr = inicioMes.ToString("yyyy-MM-dd");

            var countType = Supabase.Postgrest.Constants.CountType.Exact;

            // Dispara todas as contagens em paralelo para reduzir latência
            var totalAssistidosTask = _supabaseService.Client
                .From<Models.Patients.Assistido>()
                .Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
                .Count(countType);

            var atendimentosHojeTask = _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioHojeStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, inicioAmanhaStr)
                .Count(countType);

            var usuariosAtivosTask = _supabaseService.Client
                .From<Models.Users.Usuario>()
                .Filter(u => u.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
                .Count(countType);

            var agendamentosPendentesTask = _supabaseService.Client
                .From<Models.Agenda.Agendamento>()
                .Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
                .Filter(a => a.DataAgendamento!, Constants.Operator.GreaterThanOrEqual, hojeDateStr)
                .Count(countType);

            var atendimentosSemanaTask = _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioSemanaStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, inicioAmanhaStr)
                .Count(countType);

            var atendimentosMesTask = _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioMesStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, inicioAmanhaStr)
                .Count(countType);

            var novosAssistidosMesTask = _supabaseService.Client
                .From<Models.Patients.Assistido>()
                .Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
                .Filter(a => a.DataCadastro!, Constants.Operator.GreaterThanOrEqual, inicioMesDateStr)
                .Count(countType);

            var atendimentosComPresencaTask = _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Filter(a => a.Presenca!, Constants.Operator.Equals, (int)StatusAtendimentoEnum.PRESENCA)
                .Count(countType);

            // total de atendimentos com status definido (qualquer que seja) -> Presenca >= 1
            var totalAtendimentosTask = _supabaseService.Client
                .From<Models.Appointment.Atendimento>()
                .Filter(a => a.Presenca!, Constants.Operator.GreaterThanOrEqual, (int)StatusAtendimentoEnum.PRESENCA)
                .Count(countType);

            await Task.WhenAll(
                totalAssistidosTask,
                atendimentosHojeTask,
                usuariosAtivosTask,
                agendamentosPendentesTask,
                atendimentosSemanaTask,
                atendimentosMesTask,
                novosAssistidosMesTask,
                atendimentosComPresencaTask,
                totalAtendimentosTask
            );

            var totalAssistidos = totalAssistidosTask.Result;
            var atendimentosHoje = atendimentosHojeTask.Result;
            var usuariosAtivos = usuariosAtivosTask.Result;
            var agendamentosPendentes = agendamentosPendentesTask.Result;
            var atendimentosSemana = atendimentosSemanaTask.Result;
            var atendimentosMes = atendimentosMesTask.Result;
            var novosAssistidosMes = novosAssistidosMesTask.Result;
            var atendimentosComPresenca = atendimentosComPresencaTask.Result;
            var totalAtendimentos = totalAtendimentosTask.Result;

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
