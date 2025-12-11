using SistemaApae.Api.Models.Agenda;
using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Auth;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Users;
using SistemaApae.Api.Repositories;
using SistemaApae.Api.Services.Appointment;

namespace SistemaApae.Api.Services.Agenda;

/// <summary>
/// Serviço customizado para Agendamento com funcionalidades de relacionamento
/// </summary>
public class AgendamentoService : Service<Agendamento, AgendamentoFilterRequest>
{
    private readonly IService<AgendamentoAssistido, AgendamentoAssistidoFilterRequest> _agendamentoAssistidoService;
    private readonly IService<Assistido, AssistidoFilterRequest> _assistidoService;
    private readonly IService<Usuario, UsuarioFilterRequest> _usuarioService;
    private readonly AtendimentoService _atendimentoService;
    private readonly ILogger<AgendamentoService> _logger;

    /// <summary>
    /// Inicializa uma nova instância do AgendamentoService
    /// </summary>
    public AgendamentoService(
        IRepository<Agendamento, AgendamentoFilterRequest> repository,
        ILogger<AgendamentoService> logger,
        ISupabaseService supabaseService,
        IService<AgendamentoAssistido, AgendamentoAssistidoFilterRequest> agendamentoAssistidoService,
        IService<Assistido, AssistidoFilterRequest> assistidoService,
        IService<Usuario, UsuarioFilterRequest> usuarioService,
        AtendimentoService atendimentoService)
        : base(repository, logger)
    {
        _logger = logger;
        _agendamentoAssistidoService = agendamentoAssistidoService;
        _assistidoService = assistidoService;
        _usuarioService = usuarioService;
        _atendimentoService = atendimentoService;
    }

    /// <summary>
    /// Cria um agendamento com seus assistidos usando DTO
    /// </summary>
    public async Task<ApiResponse<AgendamentoResponseDto>> Create(AgendamentoCreateDto dto)
    {
        try
        {
            // Validar dados de entrada
            var erroValidacao = ValidarDadosEntradaCreate(dto);
            if (erroValidacao != null) return erroValidacao;

            // Validar se o profissional existe
            var erroProfissional = await ValidarProfissionalExistenteAsync(dto.Profissional);
            if (erroProfissional != null) return erroProfissional;

            // Validar e buscar assistidos
            var (erroAssistidos, assistidosValidados) = await ValidarEBuscarAssistidosAsync(dto.Assistidos);
            if (erroAssistidos != null) return erroAssistidos;

            // Criar o agendamento
            var agendamento = new Agendamento
            {
                IdProfissional = dto.Profissional.Id,
                TipoRecorrencia = dto.TipoRecorrencia,
                HorarioAgendamento = dto.HorarioAgendamento,
                DataAgendamento = dto.DataAgendamento,
                DiaSemana = dto.DiaSemana,
                Observacao = dto.Observacao
            };

            var agendamentoResult = await base.Create(agendamento);
            
            if (!agendamentoResult.Success || agendamentoResult.Data == null)
            {
                return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Erro ao criar agendamento");
            }

            // Criar os relacionamentos com os assistidos
            foreach (var assistido in assistidosValidados!)
            {
                await CriarOuReativarRelacionamentoAsync(agendamentoResult.Data.Id, assistido.Id);
            }

            // Montar o DTO de resposta
            var responseDto = new AgendamentoResponseDto
            {
                Id = agendamentoResult.Data.Id,
                Profissional = dto.Profissional,
                TipoRecorrencia = agendamentoResult.Data.TipoRecorrencia,
                HorarioAgendamento = agendamentoResult.Data.HorarioAgendamento,
                DataAgendamento = agendamentoResult.Data.DataAgendamento,
                DiaSemana = agendamentoResult.Data.DiaSemana,
                Observacao = agendamentoResult.Data.Observacao,
                Status = agendamentoResult.Data.Status,
                Assistidos = assistidosValidados.Select(a => new AssistidoDto
                {
                    Id = a.Id,
                    Nome = a.Nome
                }).ToList()
            };

            return ApiResponse<AgendamentoResponseDto>.SuccessResponse(responseDto, "Agendamento criado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao criar agendamento");
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Erro interno ao criar agendamento");
        }
    }

    /// <summary>
    /// Atualiza um agendamento e seus assistidos usando DTO
    /// </summary>
    public async Task<ApiResponse<AgendamentoResponseDto>> Update(AgendamentoUpdateDto dto)
    {
        try
        {
            // Validar dados de entrada
            var erroValidacao = await ValidarDadosEntradaUpdateAsync(dto);
            if (erroValidacao != null) return erroValidacao;

            // Atualizar o agendamento usando o método base
            var agendamento = new Agendamento
            {
                Id = dto.Id,
                IdProfissional = dto.Profissional.Id,
                TipoRecorrencia = dto.TipoRecorrencia,
                HorarioAgendamento = dto.HorarioAgendamento,
                DataAgendamento = dto.DataAgendamento,
                DiaSemana = dto.DiaSemana,
                Observacao = dto.Observacao,
                Status = dto.Status
            };

            var agendamentoResult = await base.Update(agendamento);
            
            if (!agendamentoResult.Success || agendamentoResult.Data == null)
            {
                return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Erro ao atualizar agendamento");
            }

            // Se Assistidos foi fornecido, atualizar os relacionamentos
            if (dto.Assistidos != null && dto.Assistidos.Count > 0)
            {
                // Buscar relacionamentos existentes (apenas ativos)
                var filter = new AgendamentoAssistidoFilterRequest 
                { 
                    IdAgendamento = dto.Id,
                    Status = StatusEntidadeEnum.Ativo
                };
                var existingRelationsResult = await _agendamentoAssistidoService.GetByFilters(filter);
                
                var existingRelations = existingRelationsResult.Success && existingRelationsResult.Data != null 
                    ? existingRelationsResult.Data.ToList() 
                    : new List<AgendamentoAssistido>();
                var existingAssistidoIds = existingRelations.Select(ga => ga.IdAssistido).ToHashSet();
                
                // Extrair IDs dos assistidos do DTO
                var assistidosDtoIds = dto.Assistidos.Select(a => a.Id).ToList();

                // Identificar assistidos para inativar
                var assistidosParaInativar = existingRelations
                    .Where(ga => !assistidosDtoIds.Contains(ga.IdAssistido))
                    .ToList();

                // Identificar assistidos para adicionar
                var assistidosParaAdicionar = assistidosDtoIds
                    .Where(idAssistido => !existingAssistidoIds.Contains(idAssistido))
                    .ToList();

                // Inativar relacionamentos ao invés de deletar
                foreach (var relacionamentoParaInativar in assistidosParaInativar)
                {
                    relacionamentoParaInativar.Status = StatusEntidadeEnum.Inativo;
                    await _agendamentoAssistidoService.Update(relacionamentoParaInativar);
                }

                // Adicionar novos relacionamentos ou reativar existentes
                foreach (var idAssistido in assistidosParaAdicionar)
                {
                    // Verificar se já existe um relacionamento inativo
                    var filterExistente = new AgendamentoAssistidoFilterRequest 
                    { 
                        IdAgendamento = dto.Id,
                        IdAssistido = idAssistido,
                        Status = StatusEntidadeEnum.Inativo
                    };
                    var relacionamentoInativo = await _agendamentoAssistidoService.GetByFilters(filterExistente);

                    if (relacionamentoInativo.Success && relacionamentoInativo.Data != null && relacionamentoInativo.Data.Any())
                    {
                        // Reativar relacionamento existente
                        var relInativo = relacionamentoInativo.Data.First();
                        relInativo.Status = StatusEntidadeEnum.Ativo;
                        await _agendamentoAssistidoService.Update(relInativo);
                    }
                    else
                    {
                        // Criar novo relacionamento
                        var novoRelacionamento = new AgendamentoAssistido
                        {
                            IdAgendamento = dto.Id,
                            IdAssistido = idAssistido,
                            Status = StatusEntidadeEnum.Ativo
                        };

                        await _agendamentoAssistidoService.Create(novoRelacionamento);
                    }
                }
            }

            // Buscar dados completos para o DTO de resposta
            var profissionalDto = agendamentoResult.Data.Profissional != null
                ? new UsuarioDto
                {
                    Id = agendamentoResult.Data.Profissional.Id,
                    Nome = agendamentoResult.Data.Profissional.Nome,
                    Email = agendamentoResult.Data.Profissional.Email,
                    Telefone = agendamentoResult.Data.Profissional.Telefone,
                    Perfil = agendamentoResult.Data.Profissional.Perfil,
                    RegistroProfissional = agendamentoResult.Data.Profissional.RegistroProfissional,
                    Especialidade = agendamentoResult.Data.Profissional.Especialidade,
                    Observacao = agendamentoResult.Data.Profissional.Observacao
                }
				: new UsuarioDto { Id = agendamentoResult.Data.IdProfissional };

			var relacionamentosUpdateResult = await _agendamentoAssistidoService.GetByFilters(new AgendamentoAssistidoFilterRequest
			{
				IdAgendamento = dto.Id,
				Status = StatusEntidadeEnum.Ativo
			});

			var assistidosDtos = (relacionamentosUpdateResult.Success && relacionamentosUpdateResult.Data != null)
				? relacionamentosUpdateResult.Data
					.Select(r => r.Assistido)
					.Where(a => a != null)
					.Cast<Assistido>()
					.GroupBy(a => a.Id)
					.Select(g => g.First())
					.Select(a => new AssistidoDto
					{
						Id = a.Id,
						Nome = a.Nome,
						NomeConvenio = a.Convenio?.Nome,
						NomeCidade = a.Municipio?.Nome
					})
					.ToList()
				: new List<AssistidoDto>();

            // Montar o DTO de resposta
            var responseDto = new AgendamentoResponseDto
            {
                Id = agendamentoResult.Data.Id,
                Profissional = profissionalDto,
                TipoRecorrencia = agendamentoResult.Data.TipoRecorrencia,
                HorarioAgendamento = agendamentoResult.Data.HorarioAgendamento,
                DataAgendamento = agendamentoResult.Data.DataAgendamento,
                DiaSemana = agendamentoResult.Data.DiaSemana,
                Observacao = agendamentoResult.Data.Observacao,
                Status = agendamentoResult.Data.Status,
                Assistidos = assistidosDtos
            };

            return ApiResponse<AgendamentoResponseDto>.SuccessResponse(responseDto, "Agendamento atualizado com sucesso");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao atualizar agendamento");
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Erro interno ao atualizar agendamento");
        }
    }

    /// <summary>
    /// Busca um agendamento por ID com seus assistidos
    /// </summary>
    public new async Task<ApiResponse<AgendamentoResponseDto>> GetById(Guid id)
    {
        try
        {
			// Primeiro, buscar relacionamentos e aproveitar embeds para reduzir round-trips
			var relacionamentosByIdResult = await _agendamentoAssistidoService.GetByFilters(new AgendamentoAssistidoFilterRequest
			{
				IdAgendamento = id,
				Status = StatusEntidadeEnum.Ativo
			});

			Agendamento? agendamentoEntity = relacionamentosByIdResult.Success && relacionamentosByIdResult.Data != null
				? relacionamentosByIdResult.Data.FirstOrDefault()?.Agendamento
				: null;

			// Se não veio embed do agendamento (ou não existem assistidos), buscar o agendamento diretamente
			if (agendamentoEntity == null)
			{
				var agendamentoResult = await base.GetById(id);
				if (!agendamentoResult.Success || agendamentoResult.Data == null)
				{
					return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Agendamento não encontrado");
				}
				agendamentoEntity = agendamentoResult.Data;
			}

            // Buscar dados completos para o DTO de resposta
			var profissionalDto = agendamentoEntity.Profissional != null
                ? new UsuarioDto
                {
					Id = agendamentoEntity.Profissional.Id,
					Nome = agendamentoEntity.Profissional.Nome,
					Email = agendamentoEntity.Profissional.Email,
					Telefone = agendamentoEntity.Profissional.Telefone,
					Perfil = agendamentoEntity.Profissional.Perfil,
					RegistroProfissional = agendamentoEntity.Profissional.RegistroProfissional,
					Especialidade = agendamentoEntity.Profissional.Especialidade,
					Observacao = agendamentoEntity.Profissional.Observacao
                }
				: new UsuarioDto { Id = agendamentoEntity.IdProfissional };

			var assistidosDtos = (relacionamentosByIdResult.Success && relacionamentosByIdResult.Data != null)
				? relacionamentosByIdResult.Data
					.Select(r => r.Assistido)
					.Where(a => a != null)
					.Cast<Assistido>()
					.GroupBy(a => a.Id)
					.Select(g => g.First())
					.Select(a => new AssistidoDto
					{
						Id = a.Id,
						Nome = a.Nome,
						NomeConvenio = a.Convenio?.Nome,
						NomeCidade = a.Municipio?.Nome
					})
					.ToList()
				: new List<AssistidoDto>();

            // Montar o DTO de resposta
            var responseDto = new AgendamentoResponseDto
            {
				Id = agendamentoEntity.Id,
                Profissional = profissionalDto,
				TipoRecorrencia = agendamentoEntity.TipoRecorrencia,
				HorarioAgendamento = agendamentoEntity.HorarioAgendamento,
				DataAgendamento = agendamentoEntity.DataAgendamento,
				DiaSemana = agendamentoEntity.DiaSemana,
				Observacao = agendamentoEntity.Observacao,
				Status = agendamentoEntity.Status,
                Assistidos = assistidosDtos
            };

            return ApiResponse<AgendamentoResponseDto>.SuccessResponse(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar agendamento");
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Erro interno ao buscar agendamento");
        }
    }

    /// <summary>
    /// Busca agendamentos por filtro com seus assistidos
    /// </summary>
    public new async Task<ApiResponse<IEnumerable<AgendamentoResponseDto>>> GetByFilters(AgendamentoFilterRequest filtros)
    {
        try
        {
            // Se houver filtro por assistido, buscar os IDs dos agendamentos que contêm esse assistido
            IEnumerable<Guid>? idsAgendamentosPorAssistido = null;
            if (filtros != null && filtros.IdAssistido != Guid.Empty)
            {
                var filterAssistido = new AgendamentoAssistidoFilterRequest 
                { 
                    IdAssistido = filtros.IdAssistido,
					Status = StatusEntidadeEnum.Ativo
                };
                var relacionamentosResult = await _agendamentoAssistidoService.GetByFilters(filterAssistido);
                
                if (relacionamentosResult.Success && relacionamentosResult.Data != null)
                {
                    idsAgendamentosPorAssistido = relacionamentosResult.Data
                        .Select(r => r.IdAgendamento)
                        .Distinct()
                        .ToList();
                }
                else
                {
                    // Se não encontrar relacionamentos, retornar lista vazia
                    return ApiResponse<IEnumerable<AgendamentoResponseDto>>.SuccessResponse(new List<AgendamentoResponseDto>());
                }
            }

            // Buscar os agendamentos usando o método base
            var agendamentosResult = await base.GetByFilters(filtros ?? new AgendamentoFilterRequest());
            
            if (!agendamentosResult.Success || agendamentosResult.Data == null)
            {
                // Se não encontrar relacionamentos, retornar lista vazia
                return ApiResponse<IEnumerable<AgendamentoResponseDto>>.SuccessResponse(new List<AgendamentoResponseDto>());
            }

            // Aplicar filtro adicional para agendamentos recorrentes baseado no dia da semana
            var agendamentosFiltrados = AgendamentoFilter.FilterRecurrentAppointments(
                agendamentosResult.Data,
                filtros?.DataAgendamentoInicio,
                filtros?.DataAgendamentoFim
            );

            // Se houver filtro por assistido, aplicar o filtro adicional
            if (idsAgendamentosPorAssistido != null)
            {
                agendamentosFiltrados = agendamentosFiltrados
                    .Where(a => idsAgendamentosPorAssistido.Contains(a.Id));
            }

            // Bulk: buscar relacionamentos e assistidos em lote para evitar N+1
            var responseDtos = new List<AgendamentoResponseDto>();

            var idsAgendamentos = agendamentosFiltrados.Select(a => a.Id).ToList();

            var relacionamentosTodosResult = await _agendamentoAssistidoService.GetByFilters(new AgendamentoAssistidoFilterRequest
            {
                IdsAgendamento = idsAgendamentos,
				Status = StatusEntidadeEnum.Ativo
            });

            var relacionamentos = (relacionamentosTodosResult.Success && relacionamentosTodosResult.Data != null)
                ? relacionamentosTodosResult.Data.ToList()
                : new List<AgendamentoAssistido>();

            var agendamentoParaAssistidos = relacionamentos
                .GroupBy(r => r.IdAgendamento)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .Select(r => r.Assistido)
                        .Where(a => a != null)
                        .Cast<Assistido>()
                        .GroupBy(a => a.Id)
                        .Select(grp => grp.First())
                        .ToList()
                );

            // Para cada agendamento, montar DTO com profissional e assistidos do mapa

            foreach (var agendamento in agendamentosFiltrados)
            {
                var profissionalDto = agendamento.Profissional != null
                    ? new UsuarioDto
                    {
                        Id = agendamento.Profissional.Id,
                        Nome = agendamento.Profissional.Nome,
                        Email = agendamento.Profissional.Email,
                        Telefone = agendamento.Profissional.Telefone,
                        Perfil = agendamento.Profissional.Perfil,
                        RegistroProfissional = agendamento.Profissional.RegistroProfissional,
                        Especialidade = agendamento.Profissional.Especialidade,
                        Observacao = agendamento.Profissional.Observacao
                    }
					: new UsuarioDto { Id = agendamento.IdProfissional };

                var assistidosDoAgendamento = agendamentoParaAssistidos.TryGetValue(agendamento.Id, out var listaAssistidos)
                    ? listaAssistidos
                    : new List<Assistido>();

                var assistidosDtos = assistidosDoAgendamento
                    .Select(a => new AssistidoDto
                    {
                        Id = a.Id,
                        Nome = a.Nome,
                        NomeConvenio = a.Convenio?.Nome,
                        NomeCidade = a.Municipio?.Nome
                    })
                    .ToList();

                responseDtos.Add(new AgendamentoResponseDto
                {
                    Id = agendamento.Id,
                    Profissional = profissionalDto,
                    TipoRecorrencia = agendamento.TipoRecorrencia,
                    HorarioAgendamento = agendamento.HorarioAgendamento,
                    DataAgendamento = agendamento.DataAgendamento,
                    DiaSemana = agendamento.DiaSemana,
                    Observacao = agendamento.Observacao,
                    Status = agendamento.Status,
                    Assistidos = assistidosDtos
                });
            }

            var response = ApiResponse<IEnumerable<AgendamentoResponseDto>>.SuccessResponse(responseDtos);
            response.Limit = agendamentosResult.Limit;
            response.Skip = agendamentosResult.Skip;

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro interno ao buscar agendamentos");
            return ApiResponse<IEnumerable<AgendamentoResponseDto>>.ErrorResponse("Erro interno ao buscar agendamentos");
        }
    }

    /// <summary>
    /// Busca agendamentos por profissional em uma data específica
    /// </summary>
    public async Task<ApiResponse<IEnumerable<AgendamentoResponseDto>>> GetByProfissional(Guid idProfissional, DateOnly? data)
    {
        try
        {
            var dataBase = data ?? DateOnly.FromDateTime(DateTime.Now);

            // Criar filtro com profissional e intervalo de data
            var filtros = new AgendamentoFilterRequest 
            { 
                IdProfissional = idProfissional,
                DataAgendamentoInicio = dataBase,
                DataAgendamentoFim = dataBase
            };

            var agendamentosResult = await base.GetByFilters(filtros);
            if (!agendamentosResult.Success || agendamentosResult.Data == null)
                return ApiResponse<IEnumerable<AgendamentoResponseDto>>.ErrorResponse("Nenhum agendamento encontrado para o profissional informado");

            // Aplicar filtro de recorrência para a data específica
            var agendamentosFiltrados = AgendamentoFilter.FilterRecurrentAppointments(
                agendamentosResult.Data, dataBase, dataBase
            );

            var responseDtos = new List<AgendamentoResponseDto>();

            // Bulk assistidos para os agendamentos do profissional no dia
            var idsAgendamentos = agendamentosFiltrados.Select(a => a.Id).ToList();

            var relacionamentosProfResult = await _agendamentoAssistidoService.GetByFilters(new AgendamentoAssistidoFilterRequest
            {
                IdsAgendamento = idsAgendamentos,
				Status = StatusEntidadeEnum.Ativo
            });

            var relacionamentos = (relacionamentosProfResult.Success && relacionamentosProfResult.Data != null)
                ? relacionamentosProfResult.Data.ToList()
                : new List<AgendamentoAssistido>();

            var agendamentoParaAssistidos = relacionamentos
                .GroupBy(r => r.IdAgendamento)
                .ToDictionary(
                    g => g.Key,
                    g => g
                        .Select(r => r.Assistido)
                        .Where(a => a != null)
                        .Cast<Assistido>()
                        .GroupBy(a => a.Id)
                        .Select(grp => grp.First())
                        .ToList()
                );

            // Buscar atendimentos do dia em lote para todos os agendamentos filtrados
            var atendimentosPorAgendamento = await _atendimentoService.GetByAgendamentosAndDate(idsAgendamentos, dataBase);

            foreach (var agendamento in agendamentosFiltrados)
            {
                var profissionalDto = agendamento.Profissional != null
                    ? new UsuarioDto
                    {
                        Id = agendamento.Profissional.Id,
                        Nome = agendamento.Profissional.Nome,
                        Email = agendamento.Profissional.Email,
                        Telefone = agendamento.Profissional.Telefone,
                        Perfil = agendamento.Profissional.Perfil,
                        RegistroProfissional = agendamento.Profissional.RegistroProfissional,
                        Especialidade = agendamento.Profissional.Especialidade,
                        Observacao = agendamento.Profissional.Observacao
                    }
					: new UsuarioDto { Id = agendamento.IdProfissional };

                var assistidosDoAgendamento = agendamentoParaAssistidos.TryGetValue(agendamento.Id, out var listaAssistidos)
                    ? listaAssistidos
                    : new List<Assistido>();

                var assistidosDtos = assistidosDoAgendamento
                    .Select(a => new AssistidoDto
                    {
                        Id = a.Id,
                        Nome = a.Nome,
                        NomeConvenio = a.Convenio?.Nome,
                        NomeCidade = a.Municipio?.Nome
                    })
                    .ToList();

                // Atendimentos do dia (bulk resolvido acima)
                var atendimentosDoDia = atendimentosPorAgendamento.TryGetValue(agendamento.Id, out var listaAtendimentos)
                    ? listaAtendimentos
                    : new List<AtendimentoDto>();

                responseDtos.Add(new AgendamentoResponseDto
                {
                    Id = agendamento.Id,
                    Profissional = profissionalDto,
                    TipoRecorrencia = agendamento.TipoRecorrencia,
                    HorarioAgendamento = agendamento.HorarioAgendamento,
                    DataAgendamento = agendamento.DataAgendamento,
                    DiaSemana = agendamento.DiaSemana,
                    Observacao = agendamento.Observacao,
                    Status = agendamento.Status,
                    Assistidos = assistidosDtos,
                    Atendimentos = atendimentosDoDia
                });
            }

            return ApiResponse<IEnumerable<AgendamentoResponseDto>>.SuccessResponse(responseDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar agendamentos por profissional");
            return ApiResponse<IEnumerable<AgendamentoResponseDto>>.ErrorResponse("Erro interno ao buscar agendamentos do profissional");
        }
    }


    #region Métodos Privados de Validação

    /// <summary>
    /// Valida os dados de entrada do DTO de criação
    /// </summary>
    private ApiResponse<AgendamentoResponseDto>? ValidarDadosEntradaCreate(AgendamentoCreateDto dto)
    {
        if (dto.Profissional == null || dto.Profissional.Id == Guid.Empty)
        {
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Profissional é obrigatório");
        }

        if (dto.Assistidos == null || dto.Assistidos.Count == 0)
        {
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Pelo menos um assistido deve ser selecionado");
        }

        // Validar IDs dos assistidos
        foreach (var assistidoDto in dto.Assistidos)
        {
            if (assistidoDto.Id == Guid.Empty)
            {
                return ApiResponse<AgendamentoResponseDto>.ErrorResponse($"Assistido {assistidoDto.Nome} inválido");
            }
        }

        return null;
    }

    /// <summary>
    /// Valida se o profissional existe no banco de dados
    /// </summary>
    private async Task<ApiResponse<AgendamentoResponseDto>?> ValidarProfissionalExistenteAsync(UsuarioDto profissional)
    {
        var profissionalResult = await _usuarioService.GetById(profissional.Id);
        if (!profissionalResult.Success || profissionalResult.Data == null)
        {
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse($"Profissional {profissional.Nome} não encontrado");
        }

        return null;
    }

    /// <summary>
    /// Valida e busca os assistidos no banco de dados
    /// </summary>
    private async Task<(ApiResponse<AgendamentoResponseDto>? erro, List<Assistido>? assistidos)> ValidarEBuscarAssistidosAsync(List<AssistidoDto> assistidosDtos)
    {
        var assistidosValidados = new List<Assistido>();

        foreach (var assistidoDto in assistidosDtos)
        {
            var assistidoResult = await _assistidoService.GetById(assistidoDto.Id);
            if (!assistidoResult.Success || assistidoResult.Data == null)
            {
                var erro = ApiResponse<AgendamentoResponseDto>.ErrorResponse(
                    $"Assistido '{assistidoDto.Nome}' (ID: {assistidoDto.Id}) não encontrado"
                );
                return (erro, null);
            }

            assistidosValidados.Add(assistidoResult.Data);
        }

        return (null, assistidosValidados);
    }

    /// <summary>
    /// Valida os dados de entrada do DTO de atualização
    /// </summary>
    private async Task<ApiResponse<AgendamentoResponseDto>?> ValidarDadosEntradaUpdateAsync(AgendamentoUpdateDto dto)
    {
        // Verificar se o agendamento existe
        var agendamentoExistente = await base.GetById(dto.Id);
        if (!agendamentoExistente.Success || agendamentoExistente.Data == null)
        {
            return ApiResponse<AgendamentoResponseDto>.ErrorResponse("Agendamento não encontrado");
        }

        // Se IdsAssistidos foi fornecido, validar se existem
        if (dto.Assistidos != null && dto.Assistidos.Count > 0)
        {
            foreach (var assistido in dto.Assistidos)
            {
                var assistidoResult = await _assistidoService.GetById(assistido.Id);
                if (!assistidoResult.Success || assistidoResult.Data == null)
                {
                    return ApiResponse<AgendamentoResponseDto>.ErrorResponse($"Assistido {assistido.Nome} não encontrado");
                }
            }
        }

        return null; // Sem erros
    }

    /// <summary>
    /// Cria ou reativa relacionamento entre agendamento e assistido
    /// </summary>
    private async Task CriarOuReativarRelacionamentoAsync(Guid idAgendamento, Guid idAssistido)
    {
        var filterExistente = new AgendamentoAssistidoFilterRequest 
        { 
            IdAgendamento = idAgendamento,
            IdAssistido = idAssistido,
            Status = null // Buscar independente do status
        };
        var relacionamentoExistente = await _agendamentoAssistidoService.GetByFilters(filterExistente);

        if (relacionamentoExistente.Success && relacionamentoExistente.Data != null && relacionamentoExistente.Data.Any())
        {
            // Se já existe, reativar
            var relExistente = relacionamentoExistente.Data.First();
            if (relExistente.Status != StatusEntidadeEnum.Ativo)
            {
                relExistente.Status = StatusEntidadeEnum.Ativo;
                await _agendamentoAssistidoService.Update(relExistente);
            }
        }
        else
        {
            // Se não existe, criar novo
            var relacionamento = new AgendamentoAssistido
            {
                IdAgendamento = idAgendamento,
                IdAssistido = idAssistido
            };

            var relacionamentoResult = await _agendamentoAssistidoService.Create(relacionamento);
            if (!relacionamentoResult.Success)
            {
                _logger.LogWarning($"Erro ao criar relacionamento entre agendamento {idAgendamento} e assistido {idAssistido}");
            }
        }
    }

    #endregion
}
