using SistemaApae.Api.Models.Agenda.Agendamento;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Appointments;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Repositories;

namespace SistemaApae.Api.Services.Appointment;

/// <summary>
/// Implementação de serviço do Atendimento
/// </summary>
public class AtendimentoService
{
    private readonly IService<Atendimento, AtendimentoFiltroRequest> _service;
    private readonly IRepository<Assistido, AssistidoFiltroRequest> _assistidoRepository;
    private readonly IRepository<Agendamento, AgendamentoAssistidoFilterRequest> _agendamentoRepository;
    private readonly ILogger<AtendimentoService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AtendimentoService
    /// </summary>
    public AtendimentoService(
        IService<Atendimento, AtendimentoFiltroRequest> service,
        IRepository<Assistido, AssistidoFiltroRequest> assistidoRepository,
        IRepository<Agendamento, AgendamentoAssistidoFilterRequest> agendamentoRepository,
        ILogger<AtendimentoService> logger
    )
    {
        _service = service;
        _assistidoRepository = assistidoRepository;
        _agendamentoRepository = agendamentoRepository;
        _logger = logger;
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

            var result = await _service.Create(appointment);

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

            var result = await _service.Update(appointment);

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
        var scheduling = await _agendamentoRepository.GetByIdAsync(id);

        return scheduling != null;
    }

    /// <summary>
    /// Valida se assistido existe
    /// </summary>
    private async Task<bool> PatientsExistsAsync(Guid id)
    {
        var patient = await _assistidoRepository.GetByIdAsync(id);

        return patient != null;
    }

}
