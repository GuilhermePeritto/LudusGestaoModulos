using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Classe base para parâmetros de consulta (filtros, ordenação, paginação)
    /// </summary>
    public class QueryParamsBase
    {
        /// <summary>
        /// Campos específicos a serem retornados (separados por vírgula). Ex: "Id,Nome,Email"
        /// </summary>
        [JsonPropertyName("fields")]
        public string? Fields { get; set; }

        /// <summary>
        /// Número da página (padrão: 1)
        /// </summary>
        [JsonPropertyName("page")]
        [Range(1, int.MaxValue, ErrorMessage = "A página deve ser maior que 0")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Offset inicial (padrão: 0)
        /// </summary>
        [JsonPropertyName("start")]
        [Range(0, int.MaxValue, ErrorMessage = "O offset deve ser maior ou igual a 0")]
        public int Start { get; set; } = 0;

        /// <summary>
        /// Limite de registros por página (padrão: 10, máximo: 100)
        /// </summary>
        [JsonPropertyName("limit")]
        [Range(1, 100, ErrorMessage = "O limite deve estar entre 1 e 100")]
        public int Limit { get; set; } = 10;

        /// <summary>
        /// Ordenação (ex: "nome" ou "nome desc"). Múltiplos campos separados por vírgula: "nome,email desc"
        /// </summary>
        [JsonPropertyName("sort")]
        public string? Sort { get; set; }
        
        /// <summary>
        /// Filtros como lista de objetos QueryFilter. Exemplo: [{"property": "Nome", "operator": "like", "value": "Empresa"}, {"property": "Situacao", "operator": "eq", "value": "Ativo"}]
        /// </summary>
        [JsonPropertyName("filter")]
        [FromQuery(Name = "filter")]
        [ModelBinder(typeof(QueryFilterModelBinder))]
        public List<QueryFilter>? Filter { get; set; }
    }
} 