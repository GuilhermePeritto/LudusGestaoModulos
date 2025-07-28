using System.Text.Json.Serialization;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Classe base para parâmetros de consulta (filtros, ordenação, paginação)
    /// </summary>
    public class QueryParamsBase
    {
        /// <summary>
        /// Campos específicos a serem retornados (separados por vírgula)
        /// </summary>
        public string? Fields { get; set; }

        /// <summary>
        /// Número da página (padrão: 1)
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Offset inicial (padrão: 0)
        /// </summary>
        public int Start { get; set; } = 0;

        /// <summary>
        /// Limite de registros por página (padrão: 10)
        /// </summary>
        public int Limit { get; set; } = 10;

        /// <summary>
        /// Ordenação (ex: "nome" ou "nome desc")
        /// </summary>
        public string? Sort { get; set; }
        
        /// <summary>
        /// Filtros em formato JSON string
        /// </summary>
        [JsonPropertyName("Filter")]
        public string? Filter { get; set; }

        /// <summary>
        /// Processa e retorna os filtros como lista de QueryFilter
        /// </summary>
        public List<QueryFilter>? GetFilters()
        {
            if (string.IsNullOrWhiteSpace(Filter)) 
                return null;

            try
            {
                var cleanFilter = Filter.Trim();

                // Tenta deserializar como array primeiro
                try
                {
                    var filters = JsonSerializer.Deserialize<List<QueryFilter>>(cleanFilter);
                    if (filters != null && filters.Any())
                        return filters;
                }
                catch
                {
                    // Continua para tentar como objeto único
                }

                // Tenta como objeto único
                try
                {
                    var jsonFilter = JsonSerializer.Deserialize<QueryFilter>(cleanFilter);
                    if (jsonFilter != null)
                        return new List<QueryFilter> { jsonFilter };
                }
                catch
                {
                    // Retorna null se falhar
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
} 