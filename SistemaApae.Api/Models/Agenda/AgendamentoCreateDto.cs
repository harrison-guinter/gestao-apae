using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Patients;
using SistemaApae.Api.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace SistemaApae.Api.Models.Agenda;

/// <summary>
/// DTO para criação de agendamento com assistidos
/// </summary>
public class AgendamentoCreateDto
{
    /// <summary>
    /// Profissional responsável
    /// </summary>
    [Required(ErrorMessage = "Profissional é obrigatório")]
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
    public DateOnly DataAgendamento { get; set; }

    /// <summary>
    /// Dia da semana do agendamento
    /// </summary>
    public DiaSemanaEnum? DiaSemana { get; set; }

    /// <summary>
    /// Observações sobre o agendamento
    /// </summary>
    public string? Observacao { get; set; }

    /// <summary>
    /// Lista de assistidos para este agendamento
    /// </summary>
    [Required(ErrorMessage = "Pelo menos um assistido deve ser selecionado")]
    [MinLength(1, ErrorMessage = "Pelo menos um assistido deve ser selecionado")]
    public List<AssistidoDto> Assistidos { get; set; } = new();
}
