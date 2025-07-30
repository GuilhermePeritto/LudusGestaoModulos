using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Interfaces
{
    /// <summary>
    /// Interface para validação de filtros
    /// </summary>
    public interface IValidadorFiltro
    {
        /// <summary>
        /// Valida um critério de filtro
        /// </summary>
        /// <param name="criterio">Critério a ser validado</param>
        /// <returns>True se válido, false caso contrário</returns>
        bool ValidarCriterio(CriterioFiltro criterio);

        /// <summary>
        /// Valida uma lista de critérios de filtro
        /// </summary>
        /// <param name="criterios">Critérios a serem validados</param>
        /// <returns>Lista de erros de validação</returns>
        List<string> ValidarCriterios(List<CriterioFiltro> criterios);

        /// <summary>
        /// Valida se uma propriedade existe no tipo especificado
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade</typeparam>
        /// <param name="propriedade">Nome da propriedade</param>
        /// <returns>True se a propriedade existe, false caso contrário</returns>
        bool ValidarPropriedade<TEntity>(string propriedade) where TEntity : class;

        /// <summary>
        /// Valida se um operador é suportado
        /// </summary>
        /// <param name="operador">Operador a ser validado</param>
        /// <returns>True se o operador é suportado, false caso contrário</returns>
        bool ValidarOperador(string operador);

        /// <summary>
        /// Valida se um valor é compatível com o tipo da propriedade
        /// </summary>
        /// <param name="valor">Valor a ser validado</param>
        /// <param name="tipoPropriedade">Tipo da propriedade</param>
        /// <returns>True se o valor é compatível, false caso contrário</returns>
        bool ValidarValor(object? valor, Type tipoPropriedade);
    }
} 