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
	/// Obtém as estatísticas do dashboard (somente informações mensais)
	/// </summary>
	/// <param name="year">Ano alvo (opcional, padrão: ano atual)</param>
	/// <param name="month">Mês alvo 1-12 (opcional, padrão: mês atual)</param>
	/// <returns>Resposta com as estatísticas do dashboard</returns>
	public async Task<ApiResponse<DashboardStatsDto>> GetDashboardStats(int? year = null, int? month = null)
	{
		try
		{
			var now = DateTime.Now;
			var targetYear = year ?? now.Year;
			var targetMonth = month ?? now.Month;

			var inicioMes = new DateOnly(targetYear, targetMonth, 1);
			var fimMes = inicioMes.AddMonths(1);

			// Faixas de tempo em DateTime/DateOnly para filtros
			var inicioMesDateTime = inicioMes.ToDateTime(TimeOnly.MinValue);
			var fimMesDateTime = fimMes.ToDateTime(TimeOnly.MinValue);

			// Converte datas para strings compatíveis com PostgREST (timestamp sem 'Z')
            string inicioMesStr = inicioMesDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string fimMesStr = fimMesDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            string inicioMesDateStr = inicioMes.ToString("yyyy-MM-dd");
			string fimMesDateStr = fimMes.ToString("yyyy-MM-dd");

			var countType = Supabase.Postgrest.Constants.CountType.Exact;

			// Consultas sequenciais (sem Task.WhenAll)
			var totalAssistidos = await _supabaseService.Client
				.From<Models.Patients.Assistido>()
				.Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
				.Count(countType);

			var usuariosAtivos = await _supabaseService.Client
				.From<Models.Users.Usuario>()
				.Filter(u => u.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
				.Count(countType);

			// Agendamentos ativos dentro do mês
			var agendamentosPendentes = await _supabaseService.Client
				.From<Models.Agenda.Agendamento>()
				.Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
				.Filter(a => a.DataAgendamento!, Constants.Operator.GreaterThanOrEqual, inicioMesDateStr)
				.Filter(a => a.DataAgendamento!, Constants.Operator.LessThan, fimMesDateStr)
				.Count(countType);

			// Atendimentos dentro do mês
			var atendimentosMes = await _supabaseService.Client
				.From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioMesStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, fimMesDateStr)
                .Count(countType);

			// Novos assistidos cadastrados no mês
			var novosAssistidosMes = await _supabaseService.Client
				.From<Models.Patients.Assistido>()
				.Filter(a => a.Status, Constants.Operator.Equals, (int)StatusEntidadeEnum.Ativo)
				.Filter(a => a.DataCadastro!, Constants.Operator.GreaterThanOrEqual, inicioMesDateStr)
				.Filter(a => a.DataCadastro!, Constants.Operator.LessThan, fimMesDateStr)
				.Count(countType);

			// Atendimentos com presença no mês
			var atendimentosComPresenca = await _supabaseService.Client
				.From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioMesStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, fimMesDateStr)
                .Filter(a => a.Presenca!, Constants.Operator.Equals, (int)StatusAtendimentoEnum.PRESENCA)
				.Count(countType);

			// Total de atendimentos com status definido no mês (denominador da taxa)
			var totalAtendimentos = await _supabaseService.Client
				.From<Models.Appointment.Atendimento>()
                .Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, inicioMesStr)
                .Filter(a => a.DataAtendimento!, Constants.Operator.LessThan, fimMesDateStr)
                .Filter(a => a.Presenca!, Constants.Operator.GreaterThanOrEqual, (int)StatusAtendimentoEnum.PRESENCA)
				.Count(countType);

			var taxaPresenca = totalAtendimentos > 0
				? Math.Round((decimal)atendimentosComPresenca / totalAtendimentos * 100, 2)
				: 0;

			var stats = new DashboardStatsDto
			{
				TotalAssistidos = totalAssistidos,
				UsuariosAtivos = usuariosAtivos,
				AgendamentosPendentes = agendamentosPendentes,
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
