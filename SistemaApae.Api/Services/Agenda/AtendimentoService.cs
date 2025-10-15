using SistemaApae.Api.Models.Agenda.Atendimento;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Repositories;

namespace SistemaApae.Api.Services.Agenda;

/// <summary>
/// Serviço para Atendimento com funcionalidades específicas
/// </summary>
public class AtendimentoService : Service<Atendimento, AtendimentoFilterRequest>
{
    private readonly ILogger<AtendimentoService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AtendimentoService
    /// </summary>
    public AtendimentoService(
        IRepository<Atendimento, AtendimentoFilterRequest> repository,
        ILogger<AtendimentoService> logger)
        : base(repository, logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Busca atendimentos por agendamento e data específica
    /// </summary>
    /// <param name="idAgendamento">ID do agendamento</param>
    /// <param name="data">Data do atendimento</param>
    /// <returns>Lista de atendimentos encontrados</returns>
    public async Task<ApiResponse<IEnumerable<AtendimentoDto>>> GetByAgendamentoAndDate(Guid idAgendamento, DateOnly data)
    {
        try
        {
            // Converter DateOnly para DateTime para o início e fim do dia
            var dataInicio = data.ToDateTime(TimeOnly.MinValue);
            var dataFim = data.ToDateTime(TimeOnly.MaxValue);

            // Criar filtro
            var filtros = new AtendimentoFilterRequest
            {
                IdAgendamento = idAgendamento,
                DataAtendimentoInicio = dataInicio,
                DataAtendimentoFim = dataFim
            };

            // Buscar atendimentos usando o método base
            var result = await base.GetByFilters(filtros);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<IEnumerable<AtendimentoDto>>.SuccessResponse(
                    new List<AtendimentoDto>(), 
                    "Nenhum atendimento encontrado"
                );
            }

            // Converter para DTO
            var atendimentosDto = result.Data.Select(a => new AtendimentoDto
            {
                Id = a.Id,
                IdAgendamento = a.IdAgendamento,
                IdAssistido = a.IdAssistido,
                DataAtendimento = a.DataAtendimento,
                Presenca = a.Presenca,
                Avaliacao = a.Avaliacao,
                Observacao = a.Observacao
            }).ToList();

            return ApiResponse<IEnumerable<AtendimentoDto>>.SuccessResponse(atendimentosDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar atendimentos por agendamento {IdAgendamento} e data {Data}", idAgendamento, data);
            return ApiResponse<IEnumerable<AtendimentoDto>>.ErrorResponse("Erro interno ao buscar atendimentos");
        }
    }

}

