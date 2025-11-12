using System;
using System.Collections.Generic;
using SistemaApae.Api.Models.Enums;
using SistemaApae.Api.Models.Filters;

namespace SistemaApae.Api.Models.Patients;

/// <summary>
/// Modelo para requisição de Assistido por filtros de pesquisa
/// </summary>
public class AssistidoFilterRequest : BaseFilter
{
    /// <summary>
    /// Lista de IDs para busca em lote
    /// </summary>
    public List<Guid>? Ids { get; set; }

    /// <summary>
    /// Nome do Assistido
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// CPF do Assistido
    /// </summary>

    public string? CPF { get; set; }

    /// <summary>
    /// Status do Assistido (ativo/inativo)
    /// </summary>
    public StatusEntidadeEnum Status { get; set; } = StatusEntidadeEnum.Ativo;

    /// <summary>
    /// Quando true, não aplica o filtro de Status (útil para entidades referenciadas)
    /// </summary>
    public bool IgnorarStatus { get; set; } = false;

}

