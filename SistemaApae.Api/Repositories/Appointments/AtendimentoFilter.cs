using SistemaApae.Api.Models.Appointment;
using Supabase.Postgrest;
using Supabase.Postgrest.Interfaces;

namespace SistemaApae.Api.Repositories.Appointments;

/// <summary>
/// Aplica filtros específicos para consultas de Atendimento
/// </summary>
public class AtendimentoFilter : IRepositoryFilter<Atendimento, AtendimentoFilterRequest>
{
    public IPostgrestTable<Atendimento> Apply(IPostgrestTable<Atendimento> query, AtendimentoFilterRequest filtros)
    {
        // Filtro por lista de agendamentos (busca em lote)
        if (filtros.IdsAgendamento != null && filtros.IdsAgendamento.Count > 0)
        {
            var idsAgendamento = filtros.IdsAgendamento.Select(id => id.ToString()).ToList();
            query = query.Filter(a => a.IdAgendamento, Constants.Operator.In, idsAgendamento);
        }
        // Filtro por agendamento único
        else if (filtros.IdAgendamento != Guid.Empty)
            query = query.Filter(a => a.IdAgendamento, Constants.Operator.Equals, filtros.IdAgendamento.ToString());

        if (filtros.IdAssistido != Guid.Empty)
            query = query.Filter(a => a.IdAssistido, Constants.Operator.Equals, filtros.IdAssistido.ToString());

        // Filtro por profissional (via relacionamento do agendamento)
        if (filtros.IdProfissional != Guid.Empty)
            query = query.Filter("agendamento.id_profissional", Constants.Operator.Equals, filtros.IdProfissional.ToString());

        // Filtro por município (via relacionamento do assistido)
        if (filtros.IdMunicipio != Guid.Empty)
            query = query.Filter("assistido.convenio.id_municipio", Constants.Operator.Equals, filtros.IdMunicipio.ToString());

        // Filtro por Convênio (via relacionamento do assistido)
        if (filtros.IdMunicipio != Guid.Empty)
            query = query.Filter("assistido.convenio.id", Constants.Operator.Equals, filtros.IdMunicipio.ToString());

        if (filtros.DataInicioAtendimento.HasValue && filtros.DataFimAtendimento.HasValue)
        {
            string dataInicioStr = filtros.DataInicioAtendimento.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string dataFimsStr = filtros.DataFimAtendimento.Value.ToString("yyyy-MM-dd HH:mm:ss");
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, dataInicioStr)
                         .Filter(a => a.DataAtendimento!, Constants.Operator.LessThanOrEqual, dataFimsStr);
        }
        else if (filtros.DataInicioAtendimento.HasValue)
        {
            string dataInicioStr = filtros.DataInicioAtendimento.Value.ToString("yyyy-MM-dd HH:mm:ss");
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.GreaterThanOrEqual, dataInicioStr);
        }
        else if (filtros.DataFimAtendimento.HasValue)
        {
            string dataFimsStr = filtros.DataFimAtendimento.Value.ToString("yyyy-MM-dd HH:mm:ss");
            query = query.Filter(a => a.DataAtendimento!, Constants.Operator.LessThanOrEqual, dataFimsStr);
        }

        // Filtro por presença/ausência
        if (filtros.Presencas != null && filtros.Presencas.Count > 0)
        {
            var valores = filtros.Presencas.Select(p => (int)p).ToList();
            query = query.Filter(a => a.Presenca!, Constants.Operator.In, valores);
        }
        else if (filtros.Presenca != null)
        {
            query = query.Filter(a => a.Presenca!, Constants.Operator.Equals, (int)filtros.Presenca);
        }

        return query;
    }
}