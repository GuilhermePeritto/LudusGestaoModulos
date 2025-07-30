using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Representa um filtro individual para consultas
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// Nome da propriedade a ser filtrada (ex: "Nome", "Email", "Situacao")
        /// </summary>
        [JsonPropertyName("property")]
        [Required(ErrorMessage = "A propriedade é obrigatória")]
        [StringLength(100, ErrorMessage = "A propriedade deve ter no máximo 100 caracteres")]
        public string Property { get; set; } = string.Empty;

        /// <summary>
        /// Operador de comparação. Valores válidos: "eq" (igual), "ne" (diferente), "like" (contém), "gt" (maior), "lt" (menor), "gte" (maior ou igual), "lte" (menor ou igual), "in" (contido em), "between" (entre valores)
        /// </summary>
        [JsonPropertyName("operator")]
        [StringLength(20, ErrorMessage = "O operador deve ter no máximo 20 caracteres")]
        public string Operator { get; set; } = "eq";

        /// <summary>
        /// Valor para comparação. Pode ser string, número, boolean, array (para operador "in"), ou objeto com "start" e "end" (para operador "between")
        /// </summary>
        [JsonPropertyName("value")]
        public string Value { get; set; } = string.Empty;
    }
} 