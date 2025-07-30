using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Extensions
{
    /// <summary>
    /// Extensões para facilitar o uso do sistema de QueryParams
    /// </summary>
    public static class QueryParamsExtensions
    {
        /// <summary>
        /// Aplica QueryParams em uma query
        /// </summary>
        public static ResultadoProcessamento<TEntity> AplicarQueryParams<TEntity>(
            this IQueryable<TEntity> query,
            QueryParamsBase queryParams,
            ProcessadorQueryParams processador) where TEntity : class
        {
            return processador.Processar(query, queryParams);
        }

        /// <summary>
        /// Aplica apenas filtros em uma query
        /// </summary>
        public static (IQueryable<TEntity> Query, List<CriterioFiltro> FiltrosMemoria) AplicarFiltros<TEntity>(
            this IQueryable<TEntity> query,
            QueryParamsBase queryParams,
            ProcessadorQueryParams processador) where TEntity : class
        {
            return processador.AplicarFiltros(query, queryParams);
        }

        /// <summary>
        /// Aplica filtros em memória em uma coleção
        /// </summary>
        public static IEnumerable<TEntity> AplicarFiltrosMemoria<TEntity>(
            this IEnumerable<TEntity> entidades,
            List<CriterioFiltro> filtrosMemoria,
            ProcessadorQueryParams processador) where TEntity : class
        {
            return processador.AplicarFiltrosMemoria(entidades, filtrosMemoria);
        }

        /// <summary>
        /// Aplica filtro de campos em uma coleção
        /// </summary>
        public static IEnumerable<object> AplicarFiltroCampos<TEntity>(
            this IEnumerable<TEntity> entidades,
            List<string> campos,
            ProcessadorQueryParams processador) where TEntity : class
        {
            return processador.AplicarFiltroCampos(entidades, campos);
        }

        /// <summary>
        /// Obtém campos de um QueryParams
        /// </summary>
        public static List<string> ObterCampos<TEntity>(
            this QueryParamsBase queryParams,
            ProcessadorQueryParams processador) where TEntity : class
        {
            return processador.AplicarCampos<TEntity>(queryParams);
        }

        /// <summary>
        /// Verifica se QueryParams tem filtros
        /// </summary>
        public static bool PossuiFiltros(this QueryParamsBase queryParams)
        {
            return queryParams.Filter != null && queryParams.Filter.Any();
        }

        /// <summary>
        /// Verifica se QueryParams tem ordenação
        /// </summary>
        public static bool PossuiOrdenacao(this QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Sort);
        }

        /// <summary>
        /// Verifica se QueryParams tem paginação
        /// </summary>
        public static bool PossuiPaginacao(this QueryParamsBase queryParams)
        {
            return queryParams.Page > 1 || queryParams.Start > 0 || queryParams.Limit != 10;
        }

        /// <summary>
        /// Verifica se QueryParams tem campos específicos
        /// </summary>
        public static bool PossuiCampos(this QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Fields);
        }

        /// <summary>
        /// Obtém a quantidade de filtros
        /// </summary>
        public static int ObterQuantidadeFiltros(this QueryParamsBase queryParams)
        {
            return queryParams.Filter?.Count ?? 0;
        }

        /// <summary>
        /// Obtém a lista de campos como string
        /// </summary>
        public static List<string> ObterCamposLista(this QueryParamsBase queryParams)
        {
            if (string.IsNullOrWhiteSpace(queryParams.Fields))
                return new List<string>();

            return queryParams.Fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();
        }

        /// <summary>
        /// Adiciona um filtro ao QueryParams
        /// </summary>
        public static QueryParamsBase AdicionarFiltro(this QueryParamsBase queryParams, QueryFilter filtro)
        {
            queryParams.Filter ??= new List<QueryFilter>();
            queryParams.Filter.Add(filtro);
            return queryParams;
        }

        /// <summary>
        /// Adiciona múltiplos filtros ao QueryParams
        /// </summary>
        public static QueryParamsBase AdicionarFiltros(this QueryParamsBase queryParams, IEnumerable<QueryFilter> filtros)
        {
            queryParams.Filter ??= new List<QueryFilter>();
            queryParams.Filter.AddRange(filtros);
            return queryParams;
        }

        /// <summary>
        /// Define campos no QueryParams
        /// </summary>
        public static QueryParamsBase DefinirCampos(this QueryParamsBase queryParams, params string[] campos)
        {
            queryParams.Fields = string.Join(",", campos);
            return queryParams;
        }

        /// <summary>
        /// Define ordenação no QueryParams
        /// </summary>
        public static QueryParamsBase DefinirOrdenacao(this QueryParamsBase queryParams, string campo, bool descendente = false)
        {
            queryParams.Sort = descendente ? $"{campo} desc" : campo;
            return queryParams;
        }

        /// <summary>
        /// Define paginação no QueryParams
        /// </summary>
        public static QueryParamsBase DefinirPaginacao(this QueryParamsBase queryParams, int pagina, int limite)
        {
            queryParams.Page = pagina;
            queryParams.Limit = limite;
            return queryParams;
        }

        /// <summary>
        /// Define paginação com offset no QueryParams
        /// </summary>
        public static QueryParamsBase DefinirPaginacaoComOffset(this QueryParamsBase queryParams, int offset, int limite)
        {
            queryParams.Start = offset;
            queryParams.Limit = limite;
            return queryParams;
        }

        /// <summary>
        /// Combina dois QueryParams
        /// </summary>
        public static QueryParamsBase CombinarCom(this QueryParamsBase queryParams, QueryParamsBase outro)
        {
            var resultado = new QueryParamsBase
            {
                Page = outro.Page > 1 ? outro.Page : queryParams.Page,
                Start = outro.Start > 0 ? outro.Start : queryParams.Start,
                Limit = outro.Limit != 10 ? outro.Limit : queryParams.Limit,
                Sort = !string.IsNullOrWhiteSpace(outro.Sort) ? outro.Sort : queryParams.Sort,
                Fields = !string.IsNullOrWhiteSpace(outro.Fields) ? outro.Fields : queryParams.Fields
            };

            // Combina filtros
            var todosFiltros = new List<QueryFilter>();
            if (queryParams.Filter != null)
                todosFiltros.AddRange(queryParams.Filter);
            if (outro.Filter != null)
                todosFiltros.AddRange(outro.Filter);

            if (todosFiltros.Any())
                resultado.Filter = todosFiltros;

            return resultado;
        }

        /// <summary>
        /// Cria uma cópia do QueryParams
        /// </summary>
        public static QueryParamsBase Copiar(this QueryParamsBase queryParams)
        {
            return new QueryParamsBase
            {
                Page = queryParams.Page,
                Start = queryParams.Start,
                Limit = queryParams.Limit,
                Sort = queryParams.Sort,
                Fields = queryParams.Fields,
                Filter = queryParams.Filter?.Select(f => new QueryFilter
                {
                    Property = f.Property,
                    Operator = f.Operator,
                    Value = f.Value
                }).ToList()
            };
        }

        /// <summary>
        /// Limpa todos os filtros do QueryParams
        /// </summary>
        public static QueryParamsBase LimparFiltros(this QueryParamsBase queryParams)
        {
            queryParams.Filter = null;
            return queryParams;
        }

        /// <summary>
        /// Limpa a ordenação do QueryParams
        /// </summary>
        public static QueryParamsBase LimparOrdenacao(this QueryParamsBase queryParams)
        {
            queryParams.Sort = null;
            return queryParams;
        }

        /// <summary>
        /// Limpa os campos específicos do QueryParams
        /// </summary>
        public static QueryParamsBase LimparCampos(this QueryParamsBase queryParams)
        {
            queryParams.Fields = null;
            return queryParams;
        }

        /// <summary>
        /// Limpa a paginação do QueryParams
        /// </summary>
        public static QueryParamsBase LimparPaginacao(this QueryParamsBase queryParams)
        {
            queryParams.Page = 1;
            queryParams.Start = 0;
            queryParams.Limit = 10;
            return queryParams;
        }

        /// <summary>
        /// Limpa todos os parâmetros do QueryParams
        /// </summary>
        public static QueryParamsBase LimparTudo(this QueryParamsBase queryParams)
        {
            return queryParams
                .LimparFiltros()
                .LimparOrdenacao()
                .LimparCampos()
                .LimparPaginacao();
        }
    }
} 