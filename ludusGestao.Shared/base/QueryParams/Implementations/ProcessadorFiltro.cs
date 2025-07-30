using System.Linq.Expressions;
using System.Reflection;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Models;
using LudusGestao.Shared.Domain.QueryParams.Exceptions;

namespace LudusGestao.Shared.Domain.QueryParams.Implementations
{
    /// <summary>
    /// Implementação do processador de filtros
    /// </summary>
    public class ProcessadorFiltro : IProcessadorFiltro
    {
        private readonly IValidadorFiltro _validador;

        public ProcessadorFiltro(IValidadorFiltro validador)
        {
            _validador = validador ?? throw new ArgumentNullException(nameof(validador));
        }

        /// <summary>
        /// Processa filtros e retorna query filtrada e filtros de memória
        /// </summary>
        public (IQueryable<TEntity> Query, List<CriterioFiltro> FiltrosMemoria) ProcessarFiltros<TEntity>(
            IQueryable<TEntity> query, 
            List<CriterioFiltro> criterios) where TEntity : class
        {
            if (criterios == null || !criterios.Any())
                return (query, new List<CriterioFiltro>());

            // Valida os critérios
            var erros = _validador.ValidarCriterios(criterios);
            if (erros.Any())
            {
                throw new QueryFilterException(
                    $"Erros de validação nos filtros: {string.Join("; ", erros)}", 
                    "VALIDATION_ERROR");
            }

            // Separa filtros entre banco e memória
            var (filtrosBanco, filtrosMemoria) = SepararFiltros(criterios);

            // Aplica filtros no banco
            var queryFiltrada = AplicarFiltrosBanco(query, filtrosBanco);

            return (queryFiltrada, filtrosMemoria);
        }

        /// <summary>
        /// Aplica filtros no banco de dados usando Expression Trees
        /// </summary>
        public IQueryable<TEntity> AplicarFiltrosBanco<TEntity>(
            IQueryable<TEntity> query, 
            List<CriterioFiltro> criterios) where TEntity : class
        {
            if (!criterios.Any())
                return query;

            var parametro = Expression.Parameter(typeof(TEntity), "e");
            Expression<Func<TEntity, bool>>? predicado = null;

            foreach (var criterio in criterios)
            {
                var expressao = CriarExpressaoFiltro<TEntity>(parametro, criterio);
                if (expressao != null)
                {
                    if (predicado == null)
                        predicado = expressao;
                    else
                    {
                        var bodyCombinado = Expression.AndAlso(predicado.Body, expressao.Body);
                        predicado = Expression.Lambda<Func<TEntity, bool>>(bodyCombinado, parametro);
                    }
                }
            }

            return predicado != null ? query.Where(predicado) : query;
        }

        /// <summary>
        /// Aplica filtros em memória
        /// </summary>
        public IEnumerable<TEntity> AplicarFiltrosMemoria<TEntity>(
            IEnumerable<TEntity> entidades, 
            List<CriterioFiltro> criterios) where TEntity : class
        {
            if (!criterios.Any())
                return entidades;

            return entidades.Where(entidade =>
            {
                foreach (var criterio in criterios)
                {
                    if (!AvaliarFiltroMemoria(entidade, criterio))
                        return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Cria expressão de filtro para um critério
        /// </summary>
        public Expression<Func<TEntity, bool>>? CriarExpressaoFiltro<TEntity>(
            ParameterExpression parametro, 
            CriterioFiltro criterio) where TEntity : class
        {
            if (criterio.TipoPropriedade == null)
                return null;

            // Verifica se é uma propriedade aninhada (ex: Email.Endereco)
            Expression? esquerda;
            if (criterio.Propriedade.Contains('.'))
            {
                esquerda = CriarExpressaoPropriedadeAninhada<TEntity>(parametro, criterio.Propriedade);
            }
            else
            {
                var propriedade = typeof(TEntity).GetProperty(criterio.Propriedade);
                if (propriedade == null)
                    return null;
                esquerda = Expression.Property(parametro, propriedade);
            }

            if (esquerda == null)
                return null;

            var direita = Expression.Constant(criterio.Valor, criterio.TipoPropriedade);

            Expression? expressao = criterio.Operador.ToLower() switch
            {
                "eq" => Expression.Equal(esquerda, direita),
                "neq" => Expression.NotEqual(esquerda, direita),
                "lt" => Expression.LessThan(esquerda, direita),
                "lte" => Expression.LessThanOrEqual(esquerda, direita),
                "gt" => Expression.GreaterThan(esquerda, direita),
                "gte" => Expression.GreaterThanOrEqual(esquerda, direita),
                "like" => CriarExpressaoContains(esquerda, direita),
                "startswith" => CriarExpressaoStartsWith(esquerda, direita),
                "endswith" => CriarExpressaoEndsWith(esquerda, direita),
                "null" => Expression.Equal(esquerda, Expression.Constant(null)),
                "notnull" => Expression.NotEqual(esquerda, Expression.Constant(null)),
                _ => null
            };

            return expressao != null ? Expression.Lambda<Func<TEntity, bool>>(expressao, parametro) : null;
        }

        /// <summary>
        /// Cria expressão para propriedade aninhada (ex: Email.Endereco)
        /// </summary>
        private Expression? CriarExpressaoPropriedadeAninhada<TEntity>(ParameterExpression parametro, string propriedadeAninhada) where TEntity : class
        {
            var partes = propriedadeAninhada.Split('.');
            if (partes.Length != 2)
                return null;

            var propriedadePrincipal = partes[0];
            var propriedadeSecundaria = partes[1];

            // Obtém a propriedade principal
            var propPrincipal = typeof(TEntity).GetProperty(propriedadePrincipal);
            if (propPrincipal == null)
                return null;

            // Cria expressão para a propriedade principal
            var expressaoPrincipal = Expression.Property(parametro, propPrincipal);

            // Obtém a propriedade secundária
            var propSecundaria = propPrincipal.PropertyType.GetProperty(propriedadeSecundaria);
            if (propSecundaria == null)
                return null;

            // Cria expressão para a propriedade secundária
            return Expression.Property(expressaoPrincipal, propSecundaria);
        }

        /// <summary>
        /// Cria expressão Contains
        /// </summary>
        private Expression CriarExpressaoContains(Expression esquerda, Expression direita)
        {
            var metodoContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            if (metodoContains != null)
                return Expression.Call(esquerda, metodoContains, direita);
            
            throw new QueryFilterException("Método Contains não encontrado para string", "METHOD_NOT_FOUND");
        }

        /// <summary>
        /// Cria expressão StartsWith
        /// </summary>
        private Expression CriarExpressaoStartsWith(Expression esquerda, Expression direita)
        {
            var metodoStartsWith = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            if (metodoStartsWith != null)
                return Expression.Call(esquerda, metodoStartsWith, direita);
            
            throw new QueryFilterException("Método StartsWith não encontrado para string", "METHOD_NOT_FOUND");
        }

        /// <summary>
        /// Cria expressão EndsWith
        /// </summary>
        private Expression CriarExpressaoEndsWith(Expression esquerda, Expression direita)
        {
            var metodoEndsWith = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            if (metodoEndsWith != null)
                return Expression.Call(esquerda, metodoEndsWith, direita);
            
            throw new QueryFilterException("Método EndsWith não encontrado para string", "METHOD_NOT_FOUND");
        }

        /// <summary>
        /// Avalia um filtro em memória
        /// </summary>
        private bool AvaliarFiltroMemoria<TEntity>(TEntity entidade, CriterioFiltro criterio) where TEntity : class
        {
            var propriedade = typeof(TEntity).GetProperty(criterio.Propriedade);
            if (propriedade == null)
                return false;

            var valor = propriedade.GetValue(entidade);

            return criterio.Operador.ToLower() switch
            {
                "in" => AvaliarOperadorIn(valor, criterio.Valor),
                "notin" => !AvaliarOperadorIn(valor, criterio.Valor),
                "between" => AvaliarOperadorBetween(valor, criterio.Valor),
                "contains" => AvaliarOperadorContains(valor, criterio.Valor),
                _ => false
            };
        }

        /// <summary>
        /// Avalia operador "in"
        /// </summary>
        private bool AvaliarOperadorIn(object? valor, object? valorFiltro)
        {
            if (valorFiltro is IEnumerable<object> enumeravel)
                return enumeravel.Contains(valor);
            return false;
        }

        /// <summary>
        /// Avalia operador "between"
        /// </summary>
        private bool AvaliarOperadorBetween(object? valor, object? valorFiltro)
        {
            if (valorFiltro is object[] intervalo && intervalo.Length == 2 && valor is IComparable comparavel)
            {
                var minimo = intervalo[0] as IComparable;
                var maximo = intervalo[1] as IComparable;
                
                if (minimo != null && maximo != null)
                {
                    return comparavel.CompareTo(minimo) >= 0 && comparavel.CompareTo(maximo) <= 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Avalia operador "contains"
        /// </summary>
        private bool AvaliarOperadorContains(object? valor, object? valorFiltro)
        {
            if (valor is string str && valorFiltro is string valorBusca)
                return str.Contains(valorBusca, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        /// <summary>
        /// Separa filtros entre banco de dados e memória
        /// </summary>
        private (List<CriterioFiltro> FiltrosBanco, List<CriterioFiltro> FiltrosMemoria) SepararFiltros(List<CriterioFiltro> criterios)
        {
            var filtrosBanco = new List<CriterioFiltro>();
            var filtrosMemoria = new List<CriterioFiltro>();

            foreach (var criterio in criterios)
            {
                if (criterio.PodeAplicarNoBanco)
                {
                    filtrosBanco.Add(criterio);
                }
                else if (criterio.PrecisaAplicarEmMemoria)
                {
                    filtrosMemoria.Add(criterio);
                }
            }

            return (filtrosBanco, filtrosMemoria);
        }
    }
} 