namespace LudusGestao.Shared.Domain.QueryParams.Constants
{
    /// <summary>
    /// Constantes para operadores de filtro
    /// </summary>
    public static class OperadoresFiltroConstants
    {
        /// <summary>
        /// Operadores que podem ser aplicados no banco de dados
        /// </summary>
        public static readonly HashSet<string> OperadoresBancoDados = new()
        {
            "eq", "neq", "lt", "lte", "gt", "gte", "like", "startswith", "endswith", "null", "notnull"
        };

        /// <summary>
        /// Operadores que precisam ser aplicados em memória
        /// </summary>
        public static readonly HashSet<string> OperadoresMemoria = new()
        {
            "in", "notin", "between", "contains"
        };

        /// <summary>
        /// Mapeamento de operadores string para enum
        /// </summary>
        public static readonly Dictionary<string, Enums.OperadorFiltro> MapeamentoOperadores = new()
        {
            { "eq", Enums.OperadorFiltro.Igual },
            { "neq", Enums.OperadorFiltro.Diferente },
            { "gt", Enums.OperadorFiltro.MaiorQue },
            { "gte", Enums.OperadorFiltro.MaiorOuIgual },
            { "lt", Enums.OperadorFiltro.MenorQue },
            { "lte", Enums.OperadorFiltro.MenorOuIgual },
            { "like", Enums.OperadorFiltro.Contem },
            { "startswith", Enums.OperadorFiltro.ComecaCom },
            { "endswith", Enums.OperadorFiltro.TerminaCom },
            { "null", Enums.OperadorFiltro.Nulo },
            { "notnull", Enums.OperadorFiltro.NaoNulo },
            { "in", Enums.OperadorFiltro.ContidoEm },
            { "notin", Enums.OperadorFiltro.NaoContidoEm },
            { "between", Enums.OperadorFiltro.Entre }
        };

        /// <summary>
        /// Mapeamento de enum para operadores string
        /// </summary>
        public static readonly Dictionary<Enums.OperadorFiltro, string> MapeamentoOperadoresReverso = new()
        {
            { Enums.OperadorFiltro.Igual, "eq" },
            { Enums.OperadorFiltro.Diferente, "neq" },
            { Enums.OperadorFiltro.MaiorQue, "gt" },
            { Enums.OperadorFiltro.MaiorOuIgual, "gte" },
            { Enums.OperadorFiltro.MenorQue, "lt" },
            { Enums.OperadorFiltro.MenorOuIgual, "lte" },
            { Enums.OperadorFiltro.Contem, "like" },
            { Enums.OperadorFiltro.ComecaCom, "startswith" },
            { Enums.OperadorFiltro.TerminaCom, "endswith" },
            { Enums.OperadorFiltro.Nulo, "null" },
            { Enums.OperadorFiltro.NaoNulo, "notnull" },
            { Enums.OperadorFiltro.ContidoEm, "in" },
            { Enums.OperadorFiltro.NaoContidoEm, "notin" },
            { Enums.OperadorFiltro.Entre, "between" }
        };
    }
} 