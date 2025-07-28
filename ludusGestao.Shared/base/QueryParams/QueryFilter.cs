using System.Text.Json.Serialization;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Representa um filtro individual para consultas
    /// </summary>
    public class QueryFilter
    {
        /// <summary>
        /// Nome da propriedade a ser filtrada
        /// </summary>
        [JsonPropertyName("property")]
        public string Property { get; set; } = string.Empty;

        /// <summary>
        /// Operador de comparação (padrão: "eq")
        /// </summary>
        [JsonPropertyName("operator")]
        public string Operator { get; set; } = "eq";

        /// <summary>
        /// Valor para comparação
        /// </summary>
        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
} 