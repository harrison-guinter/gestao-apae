using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Repositories;
using Supabase.Postgrest.Interfaces;
using Supabase.Postgrest;
using SistemaApae.Api.Models.Agenda;

/// <summary>
/// Filtro para agendamentos com suporte a recorrência
/// </summary>
public class AgendamentoFilter : IRepositoryFilter<Agendamento, AgendamentoFilterRequest>
{
    /// <summary>
    /// Aplica filtros de busca à query de agendamentos
    /// </summary>
    public IPostgrestTable<Agendamento> Apply(IPostgrestTable<Agendamento> query, AgendamentoFilterRequest filtros)
    {
        if (filtros == null)
            return query;

        // Filtro por profissional
        if (filtros.IdProfissional != Guid.Empty)
        {
            query = query.Where(a => a.IdProfissional! == filtros.IdProfissional);
        }

        query = query.Filter(u => u.Status, Constants.Operator.Equals, (int)filtros.Status);

        // Nota: NÃO aplicamos filtro de data aqui no banco de dados
        // porque agendamentos recorrentes não têm DataAgendamento preenchida.
        // O filtro de data será aplicado em memória no método FilterRecurrentAppointments
        // que considera tanto DataAgendamento (para agendamentos não recorrentes)
        // quanto DiaSemana (para agendamentos recorrentes)

        return query;
    }

    /// <summary>
    /// Filtra agendamentos baseados na data (para não recorrentes) e dia da semana (para recorrentes)
    /// Este método é chamado após a busca no banco de dados para filtrar os agendamentos corretamente
    /// </summary>
    public static IEnumerable<Agendamento> FilterRecurrentAppointments(
        IEnumerable<Agendamento> agendamentos, 
        DateOnly? dataInicio, 
        DateOnly? dataFim)
    {
        if (!dataInicio.HasValue && !dataFim.HasValue)
            return agendamentos;

        var resultado = new List<Agendamento>();

        foreach (var agendamento in agendamentos)
        {
            // Agendamentos sem recorrência ou com recorrência NENHUM são filtrados pela data
            if (agendamento.TipoRecorrencia == null || 
                agendamento.TipoRecorrencia == TipoRecorrenciaEnum.NENHUM)
            {
                // Verifica se a data do agendamento está dentro do intervalo
                if (agendamento.DataAgendamento.HasValue)
                {
                    bool dentroDoIntervalo = true;
                    
                    if (dataInicio.HasValue && agendamento.DataAgendamento.Value < dataInicio.Value)
                        dentroDoIntervalo = false;
                    
                    if (dataFim.HasValue && agendamento.DataAgendamento.Value > dataFim.Value)
                        dentroDoIntervalo = false;
                    
                    if (dentroDoIntervalo)
                        resultado.Add(agendamento);
                }
            }
            // Agendamentos com recorrência SEMANAL são filtrados pelo dia da semana
            else if (agendamento.TipoRecorrencia == TipoRecorrenciaEnum.SEMANAL && 
                     agendamento.DiaSemana.HasValue)
            {
                // Verifica se o dia da semana do agendamento está presente no intervalo de datas
                if (IsDayOfWeekInDateRange(agendamento.DiaSemana.Value, dataInicio, dataFim))
                {
                    resultado.Add(agendamento);
                }
            }
            else
            {
                // Outros casos (sem dia da semana definido e sem data) - inclui por padrão
                resultado.Add(agendamento);
            }
        }

        return resultado;
    }

    /// <summary>
    /// Verifica se um dia da semana específico está presente em um intervalo de datas
    /// </summary>
    private static bool IsDayOfWeekInDateRange(DiaSemanaEnum diaSemana, DateOnly? dataInicio, DateOnly? dataFim)
    {
        // Se não há intervalo de datas, inclui o agendamento
        if (!dataInicio.HasValue && !dataFim.HasValue)
            return true;

        // Converte o enum para DayOfWeek do C#
        DayOfWeek targetDayOfWeek = ConvertDiaSemanaEnumToDayOfWeek(diaSemana);

        // Define o intervalo de datas para verificação
        DateOnly inicio = dataInicio ?? DateOnly.MinValue;
        DateOnly fim = dataFim ?? DateOnly.MaxValue;

        // Verifica se o intervalo tem pelo menos 7 dias (garante que todos os dias da semana estão presentes)
        if ((fim.ToDateTime(TimeOnly.MinValue) - inicio.ToDateTime(TimeOnly.MinValue)).Days >= 6)
            return true;

        // Percorre o intervalo de datas e verifica se o dia da semana está presente
        for (DateOnly data = inicio; data <= fim; data = data.AddDays(1))
        {
            if (data.DayOfWeek == targetDayOfWeek)
                return true;
        }

        return false;
    }

    /// <summary>
    /// Converte DiaSemanaEnum para DayOfWeek do C#
    /// </summary>
    private static DayOfWeek ConvertDiaSemanaEnumToDayOfWeek(DiaSemanaEnum diaSemana)
    {
        return diaSemana switch
        {
            DiaSemanaEnum.SEGUNDA => DayOfWeek.Monday,
            DiaSemanaEnum.TERCA => DayOfWeek.Tuesday,
            DiaSemanaEnum.QUARTA => DayOfWeek.Wednesday,
            DiaSemanaEnum.QUINTA => DayOfWeek.Thursday,
            DiaSemanaEnum.SEXTA => DayOfWeek.Friday,
            _ => DayOfWeek.Monday
        };
    }
}
