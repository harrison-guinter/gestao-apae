using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace SistemaApae.Api.Models
{
    /// <summary>
    /// Base genérica para todas as entidades
    /// </summary>
    public abstract class ApiBaseModel : BaseModel
    {
        /// <summary>
        /// ID único da entidade
        /// </summary>
        [PrimaryKey("id", false)] // true indica que é auto-increment
        public Guid Id { get; set; }
    }
}
