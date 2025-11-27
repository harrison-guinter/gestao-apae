using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Appointments;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Reports.Faltas;
using SistemaApae.Api.Models.Reports.PatientsAttendance;
using SistemaApae.Api.Models.Reports.Presencas;
using SistemaApae.Api.Repositories;
using System.Globalization;

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
                Profissional = new ProfissionalAtendimentoDto(result.Data.Agendamento.Profissional.Id, result.Data.Agendamento.Profissional.Nome),
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
                        Assistido = new AssistidoAtendimentoDto(a.IdAssistido, a.Assistido!.Nome),
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

    /// <summary>
    /// Gerar o relatório de assistidos atendidos por filtros opcionais
    /// </summary>
    public async Task<ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>> GetPatientsAttendanceReport(
        AssistidosAtendidosReportFilterRequest filterRequest
    )
    {
        try
        {
            var filters = new AtendimentoFilterRequest
            {
                DataInicioAtendimento = filterRequest.DataInicio,
                DataFimAtendimento = filterRequest.DataFim,
                IdProfissional = filterRequest.IdProfissional,
                IdMunicipio = filterRequest.IdMunicipio,
                IdAssistido = filterRequest.IdAssistido
            };

            var result = await base.GetByFilters(filters);

            if (!result.Success || result.Data == null)
                return ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>.SuccessResponse(Enumerable.Empty<AssistidosAtendidosReportDto>());

            var appointments = result.Data.Select(a =>
            {
                return new AssistidosAtendidosReportDto
                {
                    Atendimento = new AtendimentoAssistidoDto(a.Id, a.DataAtendimento),
                    Profissional = new ProfissionalAtendimentoDto(a.Agendamento.Profissional.Id, a.Agendamento.Profissional.Nome),
                    Assistido = new AssistidoAtendimentoDto(a.Assistido.Id, a.Assistido.Nome),
                    Municipio = new MunicipioAssistidoDto(a.Assistido.Convenio.Municipio.Id, a.Assistido.Convenio.Municipio.Nome)
                };
            }).ToList();

            return ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>.SuccessResponse(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao gerar relatório de assistidos atendidos");
            return ApiResponse<IEnumerable<AssistidosAtendidosReportDto>>.ErrorResponse("Erro interno ao gerar relatório de assistidos atendidos");
        }
    }

    /// <summary>
    /// Relatório de Presença com metadados (mês/período, município, convênio) e itens agregados
    /// </summary>
    public async Task<ApiResponse<PresencaListaReportDto>> GetRelatorioPresencasLista(PresencaReportFilterRequest filtros)
    {
        try
        {
            var filtrosAt = new AtendimentoFilterRequest
            {
                DataInicioAtendimento = filtros.DataInicio,
                DataFimAtendimento = filtros.DataFim,
                IdProfissional = filtros.IdProfissional,
                IdMunicipio = filtros.IdMunicipio,
                IdConvenio = filtros.IdConvenio,
                Presenca = StatusAtendimentoEnum.PRESENCA
            };

            var result = await base.GetByFilters(filtrosAt);
            var atendimentos = result.Success && result.Data != null ? result.Data : Enumerable.Empty<Atendimento>();

            // Itens
            var rows = atendimentos
                .GroupBy(a => a.Assistido?.Id ?? Guid.Empty)
                .Where(g => g.Key != Guid.Empty)
                .Select(g =>
                {
                    var any = g.First();
                    var assistido = any.Assistido!;
                    var turma = assistido.NomeTurmaApae;
                    var especialidades = g.Select(x => x.Agendamento?.Profissional?.Especialidade)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .ToList();

                    var tipoAtendimentoParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(turma))
                        tipoAtendimentoParts.Add(turma!);
                    if (especialidades.Count > 0)
                        tipoAtendimentoParts.Add(string.Join(", ", especialidades));
                    var tipoAtendimento = tipoAtendimentoParts.Count > 0 ? string.Join(" , ", tipoAtendimentoParts) : string.Empty;

                    var diasOrdinais = g
                        .Select(x => x.DataAtendimento?.DayOfWeek)
                        .Where(d => d.HasValue)
                        .Select(d => GetOrdinalFromDayOfWeek(d!.Value))
                        .Distinct()
                        .OrderBy(d => d)
                        .Select(d => $"{d}º")
                        .ToList();

                    string turno = assistido.TurnoTurmaApae.HasValue
                        ? MapTurnoToSigla(assistido.TurnoTurmaApae.Value)
                        : string.Empty;

                    return new
                    {
                        Assistido = assistido,
                        TipoAtendimento = tipoAtendimento,
                        DiaTerapias = diasOrdinais.Count > 0 ? string.Join(", ", diasOrdinais) : string.Empty,
                        DiaSemana = assistido.DiasTurmaApae ?? string.Empty,
                        Turno = turno
                    };
                })
                .OrderBy(r => r.Assistido.Nome, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var itens = rows
                .Select((r, idx) => new PresencaListaItemDto
                {
                    Numero = idx + 1,
                    Nome = r.Assistido.Nome,
                    DataNascimento = r.Assistido.DataNascimento?.ToString() ?? string.Empty,
                    Endereco = r.Assistido.Endereco ?? string.Empty,
                    TipoAtendimento = r.TipoAtendimento,
                    DiaTerapias = r.DiaTerapias,
                    DiaSemana = r.DiaSemana,
                    Turno = r.Turno
                })
                .ToList();

            // Cabeçalho resolvido
            var culture = new CultureInfo("pt-BR");
            string mes = string.Empty;
            string periodo = string.Empty;
            if (filtros.DataInicio.HasValue && filtros.DataFim.HasValue &&
                filtros.DataInicio.Value.Month == filtros.DataFim.Value.Month &&
                filtros.DataInicio.Value.Year == filtros.DataFim.Value.Year)
            {
                mes = filtros.DataInicio.Value.ToString("MMMM 'de' yyyy", culture).ToUpper(culture);
            }
            else
            {
                var inicio = filtros.DataInicio?.ToString("dd/MM/yyyy") ?? string.Empty;
                var fim = filtros.DataFim?.ToString("dd/MM/yyyy") ?? string.Empty;
                periodo = (!string.IsNullOrWhiteSpace(inicio) || !string.IsNullOrWhiteSpace(fim))
                    ? $"{inicio} a {fim}".Trim()
                    : "Todos";
            }

            string convenioNome = "Todos";
            string municipioNome = "Todos";
            if (filtros.IdConvenio != Guid.Empty)
            {
                var first = atendimentos.FirstOrDefault();
                convenioNome = first?.Assistido?.Convenio?.Nome ?? string.Empty;
                municipioNome = first?.Assistido?.Convenio?.Municipio?.Nome ?? string.Empty;
            }
            else if (filtros.IdMunicipio != Guid.Empty)
            {
                var first = atendimentos.FirstOrDefault();
                municipioNome = first?.Assistido?.Convenio?.Municipio?.Nome
                                ?? string.Empty;
            }

            var dto = new PresencaListaReportDto
            {
                Mes = mes,
                Periodo = periodo,
                Municipio = municipioNome,
                Convenio = convenioNome,
                Itens = itens
            };

            return ApiResponse<PresencaListaReportDto>.SuccessResponse(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao montar relatório detalhado de presenças");
            return ApiResponse<PresencaListaReportDto>.ErrorResponse("Erro interno ao montar relatório de presenças");
        }
    }

    private static string MapTurnoToSigla(TurnoEscolaEnum turno)
    {
        return turno switch
        {
            TurnoEscolaEnum.MANHA => "M",
            TurnoEscolaEnum.TARDE => "T",
            TurnoEscolaEnum.NOITE => "N",
            TurnoEscolaEnum.INTEGRAL => "I",
            _ => string.Empty
        };
    }

    private static int GetOrdinalFromDayOfWeek(DayOfWeek day)
    {
        return day switch
        {
            DayOfWeek.Sunday => 1,
            DayOfWeek.Monday => 2,
            DayOfWeek.Tuesday => 3,
            DayOfWeek.Wednesday => 4,
            DayOfWeek.Thursday => 5,
            DayOfWeek.Friday => 6,
            DayOfWeek.Saturday => 7,
            _ => 0
        };
    }
}

