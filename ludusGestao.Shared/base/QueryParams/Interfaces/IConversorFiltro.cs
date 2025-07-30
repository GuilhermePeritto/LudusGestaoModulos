using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Interfaces
{
    /// <summary>
    /// Interface para conversão de filtros
    /// </summary>
    public interface IConversorFiltro
    {
        /// <summary>
        /// Converte QueryFilters para CriterioFiltro
        /// </summary>
        /// <param name="queryFilters">Lista de QueryFilter</param>
        /// <param name="tipoEntidade">Tipo da entidade</param>
        /// <returns>Lista de CriterioFiltro</returns>
        List<CriterioFiltro> ConverterParaCriterios(List<QueryFilter> queryFilters, Type tipoEntidade);

        /// <summary>
        /// Converte um QueryFilter para CriterioFiltro
        /// </summary>
        /// <param name="queryFilter">QueryFilter a ser convertido</param>
        /// <param name="tipoEntidade">Tipo da entidade</param>
        /// <returns>CriterioFiltro</returns>
        CriterioFiltro ConverterParaCriterio(QueryFilter queryFilter, Type tipoEntidade);

        /// <summary>
        /// Converte valor para o tipo da propriedade
        /// </summary>
        /// <param name="valor">Valor a ser convertido</param>
        /// <param name="tipoDestino">Tipo de destino</param>
        /// <returns>Valor convertido</returns>
        object? ConverterValor(object? valor, Type tipoDestino);

        /// <summary>
        /// Separa filtros entre banco de dados e memória
        /// </summary>
        /// <param name="criterios">Critérios de filtro</param>
        /// <returns>Tupla com filtros de banco e memória</returns>
        (List<CriterioFiltro> FiltrosBanco, List<CriterioFiltro> FiltrosMemoria) SepararFiltros(List<CriterioFiltro> criterios);
    }
} 