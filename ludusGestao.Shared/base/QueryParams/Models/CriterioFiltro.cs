using LudusGestao.Shared.Domain.QueryParams.Enums;

namespace LudusGestao.Shared.Domain.QueryParams.Models
{
    /// <summary>
    /// Critério de filtro interno processado
    /// </summary>
    public class CriterioFiltro
    {
        /// <summary>
        /// Nome da propriedade a ser filtrada
        /// </summary>
        public string Propriedade { get; set; } = string.Empty;

        /// <summary>
        /// Operador de comparação
        /// </summary>
        public string Operador { get; set; } = string.Empty;

        /// <summary>
        /// Valor para comparação
        /// </summary>
        public object? Valor { get; set; }

        /// <summary>
        /// Tipo da propriedade
        /// </summary>
        public Type? TipoPropriedade { get; set; }

        /// <summary>
        /// Operador como enum
        /// </summary>
        public OperadorFiltro? OperadorEnum => 
            Constants.OperadoresFiltroConstants.MapeamentoOperadores.TryGetValue(Operador.ToLower(), out var operador) 
                ? operador 
                : null;

        /// <summary>
        /// Verifica se o operador pode ser aplicado no banco de dados
        /// </summary>
        public bool PodeAplicarNoBanco => 
            Constants.OperadoresFiltroConstants.OperadoresBancoDados.Contains(Operador.ToLower());

        /// <summary>
        /// Verifica se o operador precisa ser aplicado em memória
        /// </summary>
        public bool PrecisaAplicarEmMemoria => 
            Constants.OperadoresFiltroConstants.OperadoresMemoria.Contains(Operador.ToLower());

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public CriterioFiltro() { }

        /// <summary>
        /// Construtor com parâmetros
        /// </summary>
        public CriterioFiltro(string propriedade, string operador, object? valor)
        {
            Propriedade = propriedade?.Trim() ?? string.Empty;
            Operador = operador?.Trim().ToLower() ?? "eq";
            Valor = valor;
        }

        /// <summary>
        /// Construtor com tipo da propriedade
        /// </summary>
        public CriterioFiltro(string propriedade, string operador, object? valor, Type? tipoPropriedade)
        {
            Propriedade = propriedade?.Trim() ?? string.Empty;
            Operador = operador?.Trim().ToLower() ?? "eq";
            Valor = valor;
            TipoPropriedade = tipoPropriedade;
        }
    }
} 