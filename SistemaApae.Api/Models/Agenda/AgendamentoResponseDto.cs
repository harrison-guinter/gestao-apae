using SistemaApae.Api.Models.Appointment;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Users;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// DTO de resposta de agendamento com assistidos
/// </summary>
public class AgendamentoResponseDto
{
    /// <summary>
    /// ID do agendamento
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Profissional responsável
    /// </summary>
    public UsuarioDto Profissional { get; set; }

    /// <summary>
    /// Tipo de recorrência do agendamento
    /// </summary>
    public TipoRecorrenciaEnum? TipoRecorrencia { get; set; }

    /// <summary>
    /// Horário do agendamento
    /// </summary>
    public TimeOnly? HorarioAgendamento { get; set; }

    /// <summary>
    /// Data do agendamento
    /// </summary>
    public DateOnly? DataAgendamento { get; set; }

    /// <summary>
    /// Dia da semana do agendamento
    /// </summary>
    public DiaSemanaEnum? DiaSemana { get; set; }

    /// <summary>
    /// Observações sobre o agendamento
    /// </summary>
    public string? Observacao { get; set; }

    /// <summary>
    /// Indica se o agendamento está ativo/inativo
    /// </summary>
    public StatusEntidadeEnum Status { get; set; }

    /// <summary>
    /// Lista de assistidos associados
    /// </summary>
    public List<AssistidoDto> Assistidos { get; set; } = new();
    public List<AtendimentoDto> Atendimentos { get; set; } = new();
}
