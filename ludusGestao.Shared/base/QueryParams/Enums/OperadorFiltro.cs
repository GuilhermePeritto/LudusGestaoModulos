namespace LudusGestao.Shared.Domain.QueryParams.Enums
{
    /// <summary>
    /// Enumeração dos operadores de filtro suportados
    /// </summary>
    public enum OperadorFiltro
    {
        /// <summary>
        /// Igual (equal)
        /// </summary>
        Igual = 1,
        
        /// <summary>
        /// Diferente (not equal)
        /// </summary>
        Diferente = 2,
        
        /// <summary>
        /// Maior que (greater than)
        /// </summary>
        MaiorQue = 3,
        
        /// <summary>
        /// Maior ou igual (greater than or equal)
        /// </summary>
        MaiorOuIgual = 4,
        
        /// <summary>
        /// Menor que (less than)
        /// </summary>
        MenorQue = 5,
        
        /// <summary>
        /// Menor ou igual (less than or equal)
        /// </summary>
        MenorOuIgual = 6,
        
        /// <summary>
        /// Contém (contains)
        /// </summary>
        Contem = 7,
        
        /// <summary>
        /// Começa com (starts with)
        /// </summary>
        ComecaCom = 8,
        
        /// <summary>
        /// Termina com (ends with)
        /// </summary>
        TerminaCom = 9,
        
        /// <summary>
        /// Nulo (is null)
        /// </summary>
        Nulo = 10,
        
        /// <summary>
        /// Não nulo (is not null)
        /// </summary>
        NaoNulo = 11,
        
        /// <summary>
        /// Contido em (in)
        /// </summary>
        ContidoEm = 12,
        
        /// <summary>
        /// Não contido em (not in)
        /// </summary>
        NaoContidoEm = 13,
        
        /// <summary>
        /// Entre valores (between)
        /// </summary>
        Entre = 14
    }
} 