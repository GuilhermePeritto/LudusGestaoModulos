using System.Text.Json;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Classe auxiliar para criação e manipulação de QueryParams
    /// </summary>
    public static class QueryParamsHelper
    {
        #region Métodos Base

        /// <summary>
        /// Cria um filtro com propriedade, operador e valor
        /// </summary>
        public static QueryParamsBase Filtrar(string propriedade, object? valor, string operador = "eq")
        {
            var filter = new QueryFilter 
            { 
                Property = propriedade, 
                Operator = operador, 
                Value = valor 
            };
            
            return new QueryParamsBase 
            { 
                Filter = JsonSerializer.Serialize(filter)
            };
        }

        /// <summary>
        /// Cria um filtro simples (propriedade:valor) usando operador eq por padrão
        /// </summary>
        public static QueryParamsBase FiltrarSimples(string propriedade, object? valor)
        {
            return Filtrar(propriedade, valor, "eq");
        }

        /// <summary>
        /// Cria múltiplos filtros
        /// </summary>
        public static QueryParamsBase FiltrarMultiplos(params (string propriedade, object? valor, string operador)[] filtros)
        {
            var queryFilters = filtros.Select(f => new QueryFilter 
            { 
                Property = f.propriedade, 
                Operator = f.operador, 
                Value = f.valor 
            }).ToList();
            
            return new QueryParamsBase 
            { 
                Filter = JsonSerializer.Serialize(queryFilters)
            };
        }

        #endregion

        #region Métodos de Filtro Específicos

        /// <summary>
        /// Filtra por ID
        /// </summary>
        public static QueryParamsBase FiltrarPorId(Guid id)
        {
            return Filtrar("Id", id, "eq");
        }

        /// <summary>
        /// Filtra por nome usando LIKE
        /// </summary>
        public static QueryParamsBase FiltrarPorNome(string nome)
        {
            return Filtrar("Nome", nome, "like");
        }

        /// <summary>
        /// Filtra por email usando LIKE
        /// </summary>
        public static QueryParamsBase FiltrarPorEmail(string email)
        {
            return Filtrar("Email", email, "like");
        }

        /// <summary>
        /// Filtra por situação
        /// </summary>
        public static QueryParamsBase FiltrarPorSituacao(string situacao)
        {
            return Filtrar("Situacao", situacao, "eq");
        }

        /// <summary>
        /// Filtra por situação ativa
        /// </summary>
        public static QueryParamsBase FiltrarPorAtivo()
        {
            return Filtrar("Situacao", "Ativo", "eq");
        }

        /// <summary>
        /// Filtra por situação inativa
        /// </summary>
        public static QueryParamsBase FiltrarPorInativo()
        {
            return Filtrar("Situacao", "Inativo", "eq");
        }

        /// <summary>
        /// Filtra por data de criação
        /// </summary>
        public static QueryParamsBase FiltrarPorDataCriacao(DateTime dataInicio, DateTime dataFim)
        {
            return FiltrarMultiplos(
                ("DataCriacao", dataInicio, "gte"),
                ("DataCriacao", dataFim, "lte")
            );
        }

        /// <summary>
        /// Filtra por data de alteração
        /// </summary>
        public static QueryParamsBase FiltrarPorDataAlteracao(DateTime dataInicio, DateTime dataFim)
        {
            return FiltrarMultiplos(
                ("DataAlteracao", dataInicio, "gte"),
                ("DataAlteracao", dataFim, "lte")
            );
        }

        #endregion

        #region Métodos de Ordenação

        /// <summary>
        /// Ordena por nome (ascendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorNome()
        {
            return new QueryParamsBase { Sort = "Nome" };
        }

        /// <summary>
        /// Ordena por nome (descendente)
        /// </summary>
        public static QueryParamsBase OrdenarPorNomeDesc()
        {
            return new QueryParamsBase { Sort = "Nome desc" };
        }

        /// <summary>
        /// Ordena por data de criação (mais recente primeiro)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataCriacao()
        {
            return new QueryParamsBase { Sort = "DataCriacao desc" };
        }

        /// <summary>
        /// Ordena por data de alteração (mais recente primeiro)
        /// </summary>
        public static QueryParamsBase OrdenarPorDataAlteracao()
        {
            return new QueryParamsBase { Sort = "DataAlteracao desc" };
        }

        #endregion

        #region Métodos de Paginação

        /// <summary>
        /// Define a página
        /// </summary>
        public static QueryParamsBase Pagina(int pagina)
        {
            return new QueryParamsBase { Page = pagina };
        }

        /// <summary>
        /// Define o limite de registros por página
        /// </summary>
        public static QueryParamsBase Limite(int limite)
        {
            return new QueryParamsBase { Limit = limite };
        }

        /// <summary>
        /// Define o offset (start)
        /// </summary>
        public static QueryParamsBase Offset(int offset)
        {
            return new QueryParamsBase { Start = offset };
        }

        #endregion

        #region Métodos de Campos

        /// <summary>
        /// Define campos específicos a serem retornados
        /// </summary>
        public static QueryParamsBase Campos(params string[] campos)
        {
            return new QueryParamsBase { Fields = string.Join(",", campos) };
        }

        /// <summary>
        /// Define campos específicos a serem retornados usando string
        /// </summary>
        public static QueryParamsBase Campos(string campos)
        {
            return new QueryParamsBase { Fields = campos };
        }

        /// <summary>
        /// Combina campos com outros parâmetros
        /// </summary>
        public static QueryParamsBase CombinarComCampos(QueryParamsBase queryParams, params string[] campos)
        {
            var resultado = new QueryParamsBase
            {
                Page = queryParams.Page,
                Start = queryParams.Start,
                Limit = queryParams.Limit,
                Sort = queryParams.Sort,
                Filter = queryParams.Filter,
                Fields = string.Join(",", campos)
            };

            return resultado;
        }

        /// <summary>
        /// Adiciona campos a um QueryParams existente
        /// </summary>
        public static QueryParamsBase AdicionarCampos(QueryParamsBase queryParams, params string[] campos)
        {
            var camposExistentes = string.IsNullOrWhiteSpace(queryParams.Fields) 
                ? new List<string>() 
                : queryParams.Fields.Split(',').Select(c => c.Trim()).ToList();

            camposExistentes.AddRange(campos.Where(c => !string.IsNullOrWhiteSpace(c)));

            var resultado = new QueryParamsBase
            {
                Page = queryParams.Page,
                Start = queryParams.Start,
                Limit = queryParams.Limit,
                Sort = queryParams.Sort,
                Filter = queryParams.Filter,
                Fields = string.Join(",", camposExistentes.Distinct())
            };

            return resultado;
        }

        #endregion

        #region Métodos de Combinação

        /// <summary>
        /// Combina múltiplos QueryParams
        /// </summary>
        public static QueryParamsBase Combinar(params QueryParamsBase[] queryParams)
        {
            if (queryParams == null || queryParams.Length == 0)
                return new QueryParamsBase();

            var resultado = new QueryParamsBase
            {
                Page = queryParams.FirstOrDefault(p => p.Page > 0)?.Page ?? 1,
                Start = queryParams.FirstOrDefault(p => p.Start > 0)?.Start ?? 0,
                Limit = queryParams.FirstOrDefault(p => p.Limit > 0)?.Limit ?? 10,
                Sort = queryParams.FirstOrDefault(p => !string.IsNullOrEmpty(p.Sort))?.Sort
            };

            // Combina campos (fields)
            var todosCampos = new List<string>();
            foreach (var param in queryParams)
            {
                if (!string.IsNullOrWhiteSpace(param.Fields))
                {
                    var campos = param.Fields.Split(',').Select(c => c.Trim()).Where(c => !string.IsNullOrWhiteSpace(c));
                    todosCampos.AddRange(campos);
                }
            }

            if (todosCampos.Any())
            {
                resultado.Fields = string.Join(",", todosCampos.Distinct());
            }

            // Combina filtros
            var todosFiltros = new List<QueryFilter>();
            foreach (var param in queryParams)
            {
                var filters = param.GetFilters();
                if (filters != null)
                    todosFiltros.AddRange(filters);
            }

            if (todosFiltros.Any())
            {
                resultado.Filter = JsonSerializer.Serialize(todosFiltros);
            }

            return resultado;
        }

        /// <summary>
        /// Combina filtro com ordenação
        /// </summary>
        public static QueryParamsBase FiltrarEOrdenar(QueryParamsBase filtro, QueryParamsBase ordenacao)
        {
            return Combinar(filtro, ordenacao);
        }

        /// <summary>
        /// Combina filtro com paginação
        /// </summary>
        public static QueryParamsBase FiltrarEPaginar(QueryParamsBase filtro, QueryParamsBase paginacao)
        {
            return Combinar(filtro, paginacao);
        }

        /// <summary>
        /// Combina filtro, ordenação e paginação
        /// </summary>
        public static QueryParamsBase FiltrarOrdenarEPaginar(QueryParamsBase filtro, QueryParamsBase ordenacao, QueryParamsBase paginacao)
        {
            return Combinar(filtro, ordenacao, paginacao);
        }

        #endregion

        #region Métodos de Validação

        /// <summary>
        /// Verifica se o QueryParams tem filtros
        /// </summary>
        public static bool TemFiltros(QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Filter);
        }

        /// <summary>
        /// Verifica se o QueryParams tem ordenação
        /// </summary>
        public static bool TemOrdenacao(QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Sort);
        }

        /// <summary>
        /// Verifica se o QueryParams tem paginação
        /// </summary>
        public static bool TemPaginacao(QueryParamsBase queryParams)
        {
            return queryParams.Page > 1 || queryParams.Start > 0 || queryParams.Limit != 10;
        }

        /// <summary>
        /// Verifica se o QueryParams tem campos específicos definidos
        /// </summary>
        public static bool TemCampos(QueryParamsBase queryParams)
        {
            return !string.IsNullOrWhiteSpace(queryParams.Fields);
        }

        /// <summary>
        /// Obtém a lista de campos definidos no QueryParams
        /// </summary>
        public static List<string> ObterCampos(QueryParamsBase queryParams)
        {
            if (string.IsNullOrWhiteSpace(queryParams.Fields))
                return new List<string>();

            return queryParams.Fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();
        }

        #endregion
    }
} 