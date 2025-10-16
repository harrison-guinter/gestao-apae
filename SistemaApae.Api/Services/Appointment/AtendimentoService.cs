using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Repositories;

namespace SistemaApae.Api.Services.Appointment;

/// <summary>
/// Serviço para Atendimento com funcionalidades específicas
/// </summary>
public class AtendimentoService : Service<Atendimento, AtendimentoFilterRequest>
{
    private readonly ILogger<AtendimentoService> _logger;
    private readonly IService<Assistido, AssistidoFilterRequest> _assistidoService;
    private readonly IService<Agendamento, AgendamentoFilterRequest> _agendamentoService;

    /// <summary>
    /// Inicializa uma nova instância do AtendimentoService
    /// </summary>
    public AtendimentoService(
        IRepository<Atendimento, AtendimentoFilterRequest> repository,
        IService<Agendamento, AgendamentoFilterRequest> agendamentoService,
        IService<Assistido, AssistidoFilterRequest> assistidoService,

        ILogger<AtendimentoService> logger)
        : base(repository, logger)
    {
        _logger = logger;
        _agendamentoService = agendamentoService;
        _assistidoService = assistidoService;
    }

    /// <summary>
    /// Cria um atendimento
    /// </summary>
    public async Task<ApiResponse<Atendimento>> Create(Atendimento appointment)
    {
        try
        {
            if (!await SchedulingExistsAsync(appointment.IdAgendamento))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Agendamento não existe");

            if (!await PatientsExistsAsync(appointment.IdAssistido))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Assistido não existe");

            var result = await base.Create(appointment);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao criar atendimento");
            return ApiResponse<Atendimento>.ErrorResponse("Erro interno ao criar atendimento");
        }
    }

    /// <summary>
    /// Atualiza um atendimento
    /// </summary>
    public async Task<ApiResponse<Atendimento>> Update(Atendimento appointment)
    {
        try
        {
            if (!await SchedulingExistsAsync(appointment.IdAgendamento))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Atendimento não existe");

            if (!await PatientsExistsAsync(appointment.IdAssistido))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Assistido não existe");

            var result = await base.Update(appointment);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao atualizar atendimento");
            return ApiResponse<Atendimento>.ErrorResponse("Erro interno ao atualizar atendimento");
        }
    }

    /// <summary>
    /// Valida se agendamento existe
    /// </summary>
    private async Task<bool> SchedulingExistsAsync(Guid id)
    {
        // Verifica se o Guid é válido (não é vazio)
        if (id == Guid.Empty)
            return false;

        var scheduling = await _agendamentoService.GetById(id);

        return scheduling != null;
    }

    /// <summary>
    /// Valida se assistido existe
    /// </summary>
    private async Task<bool> PatientsExistsAsync(Guid id)
    {
        // Verifica se o Guid é válido (não é vazio)
        if (id == Guid.Empty)
            return false;

        var patient = await _assistidoService.GetById(id);

        return patient != null;
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
                DataInicioAtendimento = dataInicio,
                DataFimAtendimento = dataFim
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

