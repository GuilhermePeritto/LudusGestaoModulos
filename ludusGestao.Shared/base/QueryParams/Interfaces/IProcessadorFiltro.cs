using System.Linq.Expressions;
using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Interfaces
{
    /// <summary>
    /// Interface para processamento de filtros
    /// </summary>
    public interface IProcessadorFiltro
    {
        /// <summary>
        /// Processa filtros e retorna query filtrada e filtros de memória
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade</typeparam>
        /// <param name="query">Query original</param>
        /// <param name="criterios">Critérios de filtro</param>
        /// <returns>Tupla com query filtrada e filtros de memória</returns>
        (IQueryable<TEntity> Query, List<CriterioFiltro> FiltrosMemoria) ProcessarFiltros<TEntity>(
            IQueryable<TEntity> query, 
            List<CriterioFiltro> criterios) where TEntity : class;

        /// <summary>
        /// Aplica filtros no banco de dados usando Expression Trees
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade</typeparam>
        /// <param name="query">Query original</param>
        /// <param name="criterios">Critérios de filtro</param>
        /// <returns>Query filtrada</returns>
        IQueryable<TEntity> AplicarFiltrosBanco<TEntity>(
            IQueryable<TEntity> query, 
            List<CriterioFiltro> criterios) where TEntity : class;

        /// <summary>
        /// Aplica filtros em memória
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade</typeparam>
        /// <param name="entidades">Entidades a serem filtradas</param>
        /// <param name="criterios">Critérios de filtro</param>
        /// <returns>Entidades filtradas</returns>
        IEnumerable<TEntity> AplicarFiltrosMemoria<TEntity>(
            IEnumerable<TEntity> entidades, 
            List<CriterioFiltro> criterios) where TEntity : class;

        /// <summary>
        /// Cria expressão de filtro para um critério
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade</typeparam>
        /// <param name="parametro">Parâmetro da expressão</param>
        /// <param name="criterio">Critério de filtro</param>
        /// <returns>Expressão de filtro</returns>
        Expression<Func<TEntity, bool>>? CriarExpressaoFiltro<TEntity>(
            ParameterExpression parametro, 
            CriterioFiltro criterio) where TEntity : class;
    }
} 