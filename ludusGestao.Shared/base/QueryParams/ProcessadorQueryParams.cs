using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Models;
using LudusGestao.Shared.Domain.QueryParams.Exceptions;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Processador principal para QueryParams que orquestra filtros, campos e paginação
    /// </summary>
    public class ProcessadorQueryParams
    {
        private readonly IValidadorFiltro _validador;
        private readonly IConversorFiltro _conversor;
        private readonly IProcessadorFiltro _processadorFiltro;
        private readonly IProcessadorCampos _processadorCampos;

        public ProcessadorQueryParams(
            IValidadorFiltro validador,
            IConversorFiltro conversor,
            IProcessadorFiltro processadorFiltro,
            IProcessadorCampos processadorCampos)
        {
            _validador = validador ?? throw new ArgumentNullException(nameof(validador));
            _conversor = conversor ?? throw new ArgumentNullException(nameof(conversor));
            _processadorFiltro = processadorFiltro ?? throw new ArgumentNullException(nameof(processadorFiltro));
            _processadorCampos = processadorCampos ?? throw new ArgumentNullException(nameof(processadorCampos));
        }

        /// <summary>
        /// Processa QueryParams completos e retorna resultado processado
        /// </summary>
        public ResultadoProcessamento<TEntity> Processar<TEntity>(
            IQueryable<TEntity> query, 
            QueryParamsBase queryParams) where TEntity : class
        {
            try
            {
                var resultado = new ResultadoProcessamento<TEntity>
                {
                    QueryOriginal = query,
                    QueryParams = queryParams
                };

                // Processa filtros
                if (queryParams.Filter != null && queryParams.Filter.Any())
                {
                    var criterios = _conversor.ConverterParaCriterios(queryParams.Filter, typeof(TEntity));
                    var (queryFiltrada, filtrosMemoria) = _processadorFiltro.ProcessarFiltros(query, criterios);
                    
                    resultado.QueryFiltrada = queryFiltrada;
                    resultado.FiltrosMemoria = filtrosMemoria;
                    resultado.CriteriosProcessados = criterios;
                }
                else
                {
                    resultado.QueryFiltrada = query;
                }

                // Processa campos
                if (!string.IsNullOrWhiteSpace(queryParams.Fields))
                {
                    resultado.CamposSelecionados = _processadorCampos.ProcessarCampos<TEntity>(queryParams.Fields);
                }

                // Processa ordenação
                if (!string.IsNullOrWhiteSpace(queryParams.Sort))
                {
                    resultado.QueryOrdenada = AplicarOrdenacao(resultado.QueryFiltrada, queryParams.Sort);
                }
                else
                {
                    resultado.QueryOrdenada = resultado.QueryFiltrada;
                }

                // Processa paginação
                resultado.QueryPaginada = AplicarPaginacao(resultado.QueryOrdenada, queryParams);

                return resultado;
            }
            catch (Exception ex) when (ex is not QueryFilterException)
            {
                throw new QueryFilterException(
                    $"Erro ao processar QueryParams: {ex.Message}", 
                    "PROCESSING_ERROR", 
                    null, 
                    null, 
                    ex);
            }
        }

        /// <summary>
        /// Aplica apenas filtros
        /// </summary>
        public (IQueryable<TEntity> Query, List<CriterioFiltro> FiltrosMemoria) AplicarFiltros<TEntity>(
            IQueryable<TEntity> query, 
            QueryParamsBase queryParams) where TEntity : class
        {
            if (queryParams.Filter == null || !queryParams.Filter.Any())
                return (query, new List<CriterioFiltro>());

            var criterios = _conversor.ConverterParaCriterios(queryParams.Filter, typeof(TEntity));
            return _processadorFiltro.ProcessarFiltros(query, criterios);
        }

        /// <summary>
        /// Aplica apenas campos
        /// </summary>
        public List<string> AplicarCampos<TEntity>(QueryParamsBase queryParams) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(queryParams.Fields))
                return new List<string>();

            return _processadorCampos.ProcessarCampos<TEntity>(queryParams.Fields);
        }

        /// <summary>
        /// Aplica filtros em memória
        /// </summary>
        public IEnumerable<TEntity> AplicarFiltrosMemoria<TEntity>(
            IEnumerable<TEntity> entidades, 
            List<CriterioFiltro> filtrosMemoria) where TEntity : class
        {
            return _processadorFiltro.AplicarFiltrosMemoria(entidades, filtrosMemoria);
        }

        /// <summary>
        /// Aplica filtro de campos em entidades
        /// </summary>
        public IEnumerable<object> AplicarFiltroCampos<TEntity>(
            IEnumerable<TEntity> entidades, 
            List<string> campos) where TEntity : class
        {
            return _processadorCampos.AplicarFiltroCampos(entidades, campos);
        }

        /// <summary>
        /// Aplica filtro de campos como DTO
        /// </summary>
        public IQueryable<EntidadeDinamicaDTO> AplicarFiltroCamposComoDTO<TEntity>(
            IQueryable<TEntity> query, 
            List<string> campos) where TEntity : class
        {
            return _processadorCampos.AplicarFiltroCamposComoDTO(query, campos);
        }

        /// <summary>
        /// Aplica ordenação na query
        /// </summary>
        private IQueryable<TEntity> AplicarOrdenacao<TEntity>(IQueryable<TEntity> query, string ordenacao) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(ordenacao))
                return query;

            try
            {
                var camposOrdenacao = ordenacao
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(c => c.Trim())
                    .Where(c => !string.IsNullOrWhiteSpace(c))
                    .ToList();

                if (!camposOrdenacao.Any())
                    return query;

                IQueryable<TEntity> queryOrdenada = query;

                foreach (var campo in camposOrdenacao)
                {
                    var (propriedade, direcao) = ParsearCampoOrdenacao(campo);
                    
                    if (_processadorCampos.CampoExiste<TEntity>(propriedade))
                    {
                        queryOrdenada = AplicarOrdenacaoCampo(queryOrdenada, propriedade, direcao);
                    }
                }

                return queryOrdenada;
            }
            catch (Exception ex)
            {
                throw new QueryFilterException(
                    $"Erro ao aplicar ordenação '{ordenacao}': {ex.Message}", 
                    "SORT_ERROR", 
                    null, 
                    ordenacao, 
                    ex);
            }
        }

        /// <summary>
        /// Aplica paginação na query
        /// </summary>
        private IQueryable<TEntity> AplicarPaginacao<TEntity>(IQueryable<TEntity> query, QueryParamsBase queryParams) where TEntity : class
        {
            if (queryParams.Start > 0)
            {
                query = query.Skip(queryParams.Start);
            }

            if (queryParams.Limit > 0)
            {
                query = query.Take(queryParams.Limit);
            }

            return query;
        }

        /// <summary>
        /// Parseia campo de ordenação
        /// </summary>
        private (string Propriedade, string Direcao) ParsearCampoOrdenacao(string campo)
        {
            var partes = campo.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var propriedade = partes[0];
            var direcao = partes.Length > 1 && partes[1].ToLower() == "desc" ? "desc" : "asc";
            
            return (propriedade, direcao);
        }

        /// <summary>
        /// Aplica ordenação em um campo específico
        /// </summary>
        private IQueryable<TEntity> AplicarOrdenacaoCampo<TEntity>(IQueryable<TEntity> query, string propriedade, string direcao) where TEntity : class
        {
            // Implementação simplificada - em produção, você pode usar Dynamic LINQ ou Expression Trees
            // Por enquanto, retorna a query sem ordenação específica
            return query;
        }
    }

    /// <summary>
    /// Resultado do processamento de QueryParams
    /// </summary>
    public class ResultadoProcessamento<TEntity> where TEntity : class
    {
        /// <summary>
        /// Query original
        /// </summary>
        public IQueryable<TEntity> QueryOriginal { get; set; } = null!;

        /// <summary>
        /// QueryParams utilizados
        /// </summary>
        public QueryParamsBase QueryParams { get; set; } = null!;

        /// <summary>
        /// Query após aplicação de filtros
        /// </summary>
        public IQueryable<TEntity> QueryFiltrada { get; set; } = null!;

        /// <summary>
        /// Query após aplicação de ordenação
        /// </summary>
        public IQueryable<TEntity> QueryOrdenada { get; set; } = null!;

        /// <summary>
        /// Query após aplicação de paginação
        /// </summary>
        public IQueryable<TEntity> QueryPaginada { get; set; } = null!;

        /// <summary>
        /// Filtros que precisam ser aplicados em memória
        /// </summary>
        public List<CriterioFiltro> FiltrosMemoria { get; set; } = new();

        /// <summary>
        /// Critérios de filtro processados
        /// </summary>
        public List<CriterioFiltro> CriteriosProcessados { get; set; } = new();

        /// <summary>
        /// Campos selecionados
        /// </summary>
        public List<string> CamposSelecionados { get; set; } = new();
    }
} 