using LudusGestao.Shared.Domain.QueryParams.Models;

namespace LudusGestao.Shared.Domain.QueryParams.Interfaces
{
    /// <summary>
    /// Interface para processamento e validação de campos
    /// </summary>
    public interface IProcessadorCampos
    {
        /// <summary>
        /// Processa e valida campos solicitados
        /// </summary>
        List<string> ProcessarCampos<TEntity>(string? campos) where TEntity : class;

        /// <summary>
        /// Aplica filtro de campos em uma entidade
        /// </summary>
        object AplicarFiltroCampos<TEntity>(TEntity entidade, List<string> campos) where TEntity : class;

        /// <summary>
        /// Aplica filtro de campos em uma coleção de entidades
        /// </summary>
        IEnumerable<object> AplicarFiltroCampos<TEntity>(IEnumerable<TEntity> entidades, List<string> campos) where TEntity : class;

        /// <summary>
        /// Aplica filtro de campos como DTO dinâmico
        /// </summary>
        IQueryable<EntidadeDinamicaDTO> AplicarFiltroCamposComoDTO<TEntity>(IQueryable<TEntity> query, List<string> campos) where TEntity : class;

        /// <summary>
        /// Valida campos solicitados
        /// </summary>
        List<string> ValidarCampos<TEntity>(List<string> campos) where TEntity : class;

        /// <summary>
        /// Verifica se um campo existe na entidade
        /// </summary>
        bool CampoExiste<TEntity>(string campo) where TEntity : class;
    }
} 