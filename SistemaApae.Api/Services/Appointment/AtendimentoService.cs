using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Appointments;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Reports.Faltas;
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
    /// Lista atendimentos por filtros de pesquisa
    /// </summary>
    public async Task<ApiResponse<IEnumerable<AtendimentoDto>>> GetByFilters(AtendimentoFilterRequest filters)
    {
        try
        {
            var result = await base.GetByFilters(filters);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<IEnumerable<AtendimentoDto>>.ErrorResponse("Registros não foram encontrados");
            }

            var response = result.Data.Select(atendimento => new AtendimentoDto
            {
                Id = atendimento.Id,
                IdAgendamento = atendimento.IdAgendamento,
                Assistido = new AssistidoAtendimentoDto(atendimento.Assistido!.Id, atendimento.Assistido.Nome),
                Profissional = new ProfissionalAtendimentoDto(atendimento.Agendamento.Profissional.Id, atendimento.Agendamento.Profissional.Nome),
                DataAtendimento = atendimento.DataAtendimento,
                Presenca = atendimento.Presenca,
                Avaliacao = atendimento.Avaliacao,
                Observacao = atendimento.Observacao
            }).ToList();

            return ApiResponse<IEnumerable<AtendimentoDto>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar atendimentos por filtros");
            return ApiResponse<IEnumerable<AtendimentoDto>>.ErrorResponse("Erro interno ao buscar atendimentos por filtros");
        }
    }

    /// <summary>
    /// Buscar um atendimento por id
    /// </summary>
    public async Task<ApiResponse<AtendimentoDto>> GetById(Guid id)
    {
        try
        {
            var result = await base.GetById(id);

            if (!result.Success || result.Data == null)
            {
                return ApiResponse<AtendimentoDto>.ErrorResponse("Registro não foi encontrado");
            }

            var response = new AtendimentoDto
            {
                Id = result.Data.Id,
                IdAgendamento = result.Data.IdAgendamento,
                Assistido = new AssistidoAtendimentoDto(result.Data.Assistido!.Id, result.Data.Assistido.Nome),
                Profissional = new ProfissionalAtendimentoDto(result.Data.Agendamento.Profissional!.Id, result.Data.Agendamento.Profissional!.Nome),
                DataAtendimento = result.Data.DataAtendimento,
                Presenca = result.Data.Presenca,
                Avaliacao = result.Data.Avaliacao,
                Observacao = result.Data.Observacao
            };

            return ApiResponse<AtendimentoDto>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar atendimento por id");
            return ApiResponse<AtendimentoDto>.ErrorResponse("Erro interno ao buscar atendimento por id");
        }
    }

    /// <summary>
    /// Cria um atendimento
    /// </summary>
    public async Task<ApiResponse<Atendimento>> Create(AtendimentoCreateDto dto)
    {
        try
        {
            if (!await SchedulingExistsAsync(dto.Agendamento.Id))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Agendamento não existe");

            if (!await PatientsExistsAsync(dto.Assistido.Id))
                return ApiResponse<Atendimento>.ErrorResponse("Id do Assistido não existe");

            var appointment = new Atendimento
            {
                IdAgendamento = dto.Agendamento.Id,
                IdAssistido = dto.Assistido.Id,
                DataAtendimento = dto.DataAtendimento,
                Presenca = dto.Presenca,
                Avaliacao = dto.Avaliacao,
                Observacao = dto.Observacao
            };

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

            var response = result.Data.Select(atendimento => new AtendimentoDto
            {
                Id = atendimento.Id,
                IdAgendamento = atendimento.IdAgendamento,
                Assistido = new AssistidoAtendimentoDto(atendimento.Assistido!.Id, atendimento.Assistido.Nome),
                DataAtendimento = atendimento.DataAtendimento,
                Presenca = atendimento.Presenca,
                Avaliacao = atendimento.Avaliacao,
                Observacao = atendimento.Observacao
            }).ToList();

            return ApiResponse<IEnumerable<AtendimentoDto>>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar atendimentos por agendamento {IdAgendamento} e data {Data}", idAgendamento, data);
            return ApiResponse<IEnumerable<AtendimentoDto>>.ErrorResponse("Erro interno ao buscar atendimentos");
        }
    }

    /// <summary>
    /// Busca atendimentos por uma lista de agendamentos em uma data específica (bulk)
    /// </summary>
    /// <param name="idsAgendamento">IDs dos agendamentos</param>
    /// <param name="data">Data do atendimento</param>
    /// <returns>Dicionário mapeando IdAgendamento -> lista de atendimentos</returns>
    public async Task<Dictionary<Guid, List<AtendimentoDto>>> GetByAgendamentosAndDate(
        IEnumerable<Guid> idsAgendamento,
        DateOnly data)
    {
        var ids = idsAgendamento?.Distinct().ToList() ?? new List<Guid>();
        if (ids.Count == 0)
            return new Dictionary<Guid, List<AtendimentoDto>>();

        try
        {
            var dataInicio = data.ToDateTime(TimeOnly.MinValue);
            var dataFim = data.ToDateTime(TimeOnly.MaxValue);

            var filtros = new AtendimentoFilterRequest
            {
                IdsAgendamento = ids,
                DataInicioAtendimento = dataInicio,
                DataFimAtendimento = dataFim
            };

            var result = await base.GetByFilters(filtros);
            var vazia = new Dictionary<Guid, List<AtendimentoDto>>();

            if (!result.Success || result.Data == null)
                return vazia;

            return result.Data
                .GroupBy(a => a.IdAgendamento)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(a => new AtendimentoDto
                    {
                        Id = a.Id,
                        IdAgendamento = a.IdAgendamento,
                        DataAtendimento = a.DataAtendimento,
                        Presenca = a.Presenca,
                        Avaliacao = a.Avaliacao,
                        Observacao = a.Observacao
                    }).ToList()
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar atendimentos em lote por data");
            return new Dictionary<Guid, List<AtendimentoDto>>();
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
	/// Gera o relatório de faltas por paciente com filtros opcionais
	/// </summary>
	public async Task<ApiResponse<IEnumerable<FaltaReportItemDto>>> GetRelatorioFaltas(FaltaReportFilterRequest filtrosRelatorio)
	{
		try
		{
			// Mapeia o filtro específico do relatório para o filtro genérico de atendimento
			var filtros = new AtendimentoFilterRequest
			{
				DataInicioAtendimento = filtrosRelatorio.DataInicio,
				DataFimAtendimento = filtrosRelatorio.DataFim,
				IdProfissional = filtrosRelatorio.IdProfissional,
				IdMunicipio = filtrosRelatorio.IdMunicipio,
				IdAssistido = filtrosRelatorio.IdAssistido,
                IdConvenio = filtrosRelatorio.IdConvenio,
				Presencas = new List<StatusAtendimentoEnum>
				{
					StatusAtendimentoEnum.FALTA,
					StatusAtendimentoEnum.JUSTIFICADA
				},
				Limit = filtrosRelatorio.Limit,
				Skip = filtrosRelatorio.Skip
			};

			var result = await base.GetByFilters(filtros);

			if (!result.Success || result.Data == null)
				return ApiResponse<IEnumerable<FaltaReportItemDto>>.SuccessResponse(Enumerable.Empty<FaltaReportItemDto>());

			var itens = result.Data.Select(at =>
			{
				var municipio = at.Assistido?.Convenio?.Municipio;
				return new FaltaReportItemDto
				{
					DataAtendimento = at.DataAtendimento,
					StatusFrequencia = at.Presenca,
					NomeAssistido = at.Assistido?.Nome,
					NomeMunicipio = municipio?.Nome,
					NomeProfissional = at.Agendamento?.Profissional?.Nome,
					NomeConvenio = at.Assistido?.Convenio?.Nome,
                    ObservacaoAtendimento = at.Observacao
                };
			}).ToList();

			return ApiResponse<IEnumerable<FaltaReportItemDto>>.SuccessResponse(itens);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Erro ao gerar relatório de faltas");
			return ApiResponse<IEnumerable<FaltaReportItemDto>>.ErrorResponse("Erro interno ao gerar relatório de faltas");
		}
	}
}

