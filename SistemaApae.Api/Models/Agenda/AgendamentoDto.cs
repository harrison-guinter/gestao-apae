using SistemaApae.Api.Models.Enums;

namespace SistemaApae.Api.Models.Agenda;
public class AgendamentoDto
{
    /// <summary>
    /// ID único da entidade
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// ID do profissional responsável
    /// </summary>
    public Guid? IdProfissional { get; set; }

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
}

