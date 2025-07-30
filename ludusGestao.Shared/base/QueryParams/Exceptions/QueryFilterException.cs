namespace LudusGestao.Shared.Domain.QueryParams.Exceptions
{
    /// <summary>
    /// Exceção específica para erros relacionados a QueryFilter
    /// </summary>
    public class QueryFilterException : Exception
    {
        /// <summary>
        /// Código do erro
        /// </summary>
        public string CodigoErro { get; }

        /// <summary>
        /// Propriedade que causou o erro
        /// </summary>
        public string? Propriedade { get; }

        /// <summary>
        /// Valor que causou o erro
        /// </summary>
        public object? Valor { get; }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public QueryFilterException(string mensagem) : base(mensagem)
        {
            CodigoErro = "QUERY_FILTER_ERROR";
        }

        /// <summary>
        /// Construtor com código de erro
        /// </summary>
        public QueryFilterException(string mensagem, string codigoErro) : base(mensagem)
        {
            CodigoErro = codigoErro;
        }

        /// <summary>
        /// Construtor com propriedade e valor
        /// </summary>
        public QueryFilterException(string mensagem, string codigoErro, string propriedade, object? valor) : base(mensagem)
        {
            CodigoErro = codigoErro;
            Propriedade = propriedade;
            Valor = valor;
        }

        /// <summary>
        /// Construtor com inner exception
        /// </summary>
        public QueryFilterException(string mensagem, Exception innerException) : base(mensagem, innerException)
        {
            CodigoErro = "QUERY_FILTER_ERROR";
        }

        /// <summary>
        /// Construtor completo
        /// </summary>
        public QueryFilterException(string mensagem, string codigoErro, string propriedade, object? valor, Exception innerException) : base(mensagem, innerException)
        {
            CodigoErro = codigoErro;
            Propriedade = propriedade;
            Valor = valor;
        }
    }
} 