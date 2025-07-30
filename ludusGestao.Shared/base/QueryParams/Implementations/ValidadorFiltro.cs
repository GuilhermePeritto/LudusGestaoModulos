using System.Reflection;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Models;
using LudusGestao.Shared.Domain.QueryParams.Exceptions;

namespace LudusGestao.Shared.Domain.QueryParams.Implementations
{
    /// <summary>
    /// Implementação do validador de filtros
    /// </summary>
    public class ValidadorFiltro : IValidadorFiltro
    {
        /// <summary>
        /// Valida um critério de filtro
        /// </summary>
        public bool ValidarCriterio(CriterioFiltro criterio)
        {
            if (criterio == null)
                return false;

            if (string.IsNullOrWhiteSpace(criterio.Propriedade))
                return false;

            if (!ValidarOperador(criterio.Operador))
                return false;

            if (criterio.TipoPropriedade != null && !ValidarValor(criterio.Valor, criterio.TipoPropriedade))
                return false;

            return true;
        }

        /// <summary>
        /// Valida uma lista de critérios de filtro
        /// </summary>
        public List<string> ValidarCriterios(List<CriterioFiltro> criterios)
        {
            var erros = new List<string>();

            if (criterios == null || !criterios.Any())
                return erros;

            foreach (var criterio in criterios)
            {
                if (!ValidarCriterio(criterio))
                {
                    erros.Add($"Critério inválido: Propriedade='{criterio.Propriedade}', Operador='{criterio.Operador}', Valor='{criterio.Valor}'");
                }
            }

            return erros;
        }

        /// <summary>
        /// Valida se uma propriedade existe no tipo especificado
        /// </summary>
        public bool ValidarPropriedade<TEntity>(string propriedade) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(propriedade))
                return false;

            var tipoEntidade = typeof(TEntity);
            var prop = tipoEntidade.GetProperty(propriedade, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            return prop != null;
        }

        /// <summary>
        /// Valida se um operador é suportado
        /// </summary>
        public bool ValidarOperador(string operador)
        {
            if (string.IsNullOrWhiteSpace(operador))
                return false;

            var operadorLower = operador.ToLower();
            return Constants.OperadoresFiltroConstants.OperadoresBancoDados.Contains(operadorLower) ||
                   Constants.OperadoresFiltroConstants.OperadoresMemoria.Contains(operadorLower);
        }

        /// <summary>
        /// Valida se um valor é compatível com o tipo da propriedade
        /// </summary>
        public bool ValidarValor(object? valor, Type tipoPropriedade)
        {
            if (valor == null)
                return true; // Valores nulos são sempre válidos

            try
            {
                // Se o tipo já é compatível, é válido
                if (tipoPropriedade.IsAssignableFrom(valor.GetType()))
                    return true;

                // Tenta converter o valor
                Convert.ChangeType(valor, tipoPropriedade);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Valida e obtém a propriedade do tipo especificado
        /// </summary>
        public PropertyInfo? ObterPropriedade<TEntity>(string propriedade) where TEntity : class
        {
            if (!ValidarPropriedade<TEntity>(propriedade))
                return null;

            var tipoEntidade = typeof(TEntity);
            return tipoEntidade.GetProperty(propriedade, 
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        /// <summary>
        /// Valida se um operador pode ser aplicado no banco de dados
        /// </summary>
        public bool PodeAplicarNoBanco(string operador)
        {
            return Constants.OperadoresFiltroConstants.OperadoresBancoDados.Contains(operador.ToLower());
        }

        /// <summary>
        /// Valida se um operador precisa ser aplicado em memória
        /// </summary>
        public bool PrecisaAplicarEmMemoria(string operador)
        {
            return Constants.OperadoresFiltroConstants.OperadoresMemoria.Contains(operador.ToLower());
        }
    }
} 