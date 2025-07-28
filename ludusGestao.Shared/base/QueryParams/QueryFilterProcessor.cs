using System.Linq.Expressions;
using System.Reflection;
using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.QueryParams
{
    /// <summary>
    /// Processador responsável por aplicar filtros em queries
    /// </summary>
    public static class QueryFilterProcessor
    {
        /// <summary>
        /// Critérios de filtro internos
        /// </summary>
        public class FilterCriteria
        {
            public string Property { get; set; } = string.Empty;
            public string Operator { get; set; } = string.Empty;
            public object? Value { get; set; }
        }

        // Operadores suportados no banco de dados
        private static readonly HashSet<string> DatabaseOperators = new()
        {
            "eq", "neq", "lt", "lte", "gt", "gte", "like", "startswith", "endswith", "null", "notnull"
        };

        // Operadores que precisam ser aplicados em memória
        private static readonly HashSet<string> MemoryOperators = new()
        {
            "in", "notin", "between", "contains"
        };

        /// <summary>
        /// Processa filtros e retorna query filtrada e filtros de memória
        /// </summary>
        public static (IQueryable<TEntity> Query, List<FilterCriteria> MemoryFilters) ProcessFilters<TEntity>(
            IQueryable<TEntity> query, 
            QueryParamsBase queryParams) where TEntity : class
        {
            var filters = queryParams.GetFilters();
            if (filters == null || !filters.Any())
                return (query, new List<FilterCriteria>());

            var filterCriteria = ConvertToFilterCriteria(filters);
            var (databaseFilters, memoryFilters) = SeparateFilters(filterCriteria);

            var filteredQuery = ApplyDatabaseFilters(query, databaseFilters);

            return (filteredQuery, memoryFilters);
        }

        /// <summary>
        /// Processa campos específicos e retorna lista de propriedades válidas
        /// </summary>
        public static List<string> ProcessFields<TEntity>(QueryParamsBase queryParams) where TEntity : class
        {
            if (string.IsNullOrWhiteSpace(queryParams.Fields))
                return new List<string>();

            var requestedFields = queryParams.Fields
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(f => f.Trim())
                .Where(f => !string.IsNullOrWhiteSpace(f))
                .ToList();

            if (!requestedFields.Any())
                return new List<string>();

            var entityType = typeof(TEntity);
            var validFields = new List<string>();

            foreach (var field in requestedFields)
            {
                var prop = entityType.GetProperty(field, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (prop != null)
                {
                    validFields.Add(prop.Name); // Usa o nome exato da propriedade
                }
            }

            return validFields;
        }

        /// <summary>
        /// Aplica filtro de campos em uma entidade, retornando apenas os campos solicitados
        /// </summary>
        public static object ApplyFieldsFilter<TEntity>(TEntity entity, List<string> fields) where TEntity : class
        {
            if (entity == null || !fields.Any())
                return entity;

            var entityType = typeof(TEntity);
            var result = new Dictionary<string, object>();

            foreach (var field in fields)
            {
                var prop = entityType.GetProperty(field, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (prop != null)
                {
                    var value = prop.GetValue(entity);
                    result[field] = value;
                }
            }

            return result;
        }

        /// <summary>
        /// Aplica filtro de campos em uma coleção de entidades
        /// </summary>
        public static IEnumerable<object> ApplyFieldsFilter<TEntity>(IEnumerable<TEntity> entities, List<string> fields) where TEntity : class
        {
            if (!fields.Any())
                return entities.Cast<object>();

            return entities.Select(entity => ApplyFieldsFilter(entity, fields));
        }



        /// <summary>
        /// Aplica filtro de campos de forma performática retornando DynamicEntityDTO
        /// </summary>
        public static IQueryable<DynamicEntityDTO> ApplyFieldsFilterAsDTO<TEntity>(IQueryable<TEntity> query, List<string> fields) where TEntity : class
        {
            if (!fields.Any())
                return query.Select(e => DynamicEntityDTO.FromEntity(e, new List<string>()));

            // Para compatibilidade com Entity Framework, vamos usar uma abordagem mais simples
            return query.Select(e => DynamicEntityDTO.FromEntity(e, fields));
        }

        /// <summary>
        /// Converte QueryFilters para FilterCriteria
        /// </summary>
        private static List<FilterCriteria> ConvertToFilterCriteria(List<QueryFilter> queryFilters)
        {
            var filters = new List<FilterCriteria>();

            foreach (var filter in queryFilters)
            {
                if (string.IsNullOrWhiteSpace(filter.Property))
                    continue;

                filters.Add(new FilterCriteria
                {
                    Property = filter.Property.Trim(),
                    Operator = (filter.Operator ?? "eq").Trim().ToLower(),
                    Value = filter.Value
                });
            }

            return filters;
        }

        /// <summary>
        /// Separa filtros entre banco de dados e memória
        /// </summary>
        private static (List<FilterCriteria> DatabaseFilters, List<FilterCriteria> MemoryFilters) SeparateFilters(List<FilterCriteria> filters)
        {
            var databaseFilters = new List<FilterCriteria>();
            var memoryFilters = new List<FilterCriteria>();

            foreach (var filter in filters)
            {
                if (CanFilterInDatabase(filter.Operator))
                {
                    databaseFilters.Add(filter);
                }
                else
                {
                    memoryFilters.Add(filter);
                }
            }

            return (databaseFilters, memoryFilters);
        }

        /// <summary>
        /// Verifica se o operador pode ser aplicado no banco de dados
        /// </summary>
        private static bool CanFilterInDatabase(string operator_)
        {
            return DatabaseOperators.Contains(operator_.ToLower());
        }

        /// <summary>
        /// Aplica filtros no banco de dados usando Expression Trees
        /// </summary>
        private static IQueryable<TEntity> ApplyDatabaseFilters<TEntity>(
            IQueryable<TEntity> query, 
            List<FilterCriteria> filters) where TEntity : class
        {
            if (!filters.Any())
                return query;

            var param = Expression.Parameter(typeof(TEntity), "e");
            Expression<Func<TEntity, bool>>? predicate = null;

            foreach (var filter in filters)
            {
                var prop = typeof(TEntity).GetProperty(filter.Property, 
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
                if (prop == null)
                    continue; // Ignora propriedades inexistentes

                var left = Expression.Property(param, prop);
                var convertedValue = ConvertValue(filter.Value, prop.PropertyType);
                var right = Expression.Constant(convertedValue);
                
                Expression expr = filter.Operator.ToLower() switch
                {
                    "eq" => Expression.Equal(left, right),
                    "neq" => Expression.NotEqual(left, right),
                    "lt" => Expression.LessThan(left, right),
                    "lte" => Expression.LessThanOrEqual(left, right),
                    "gt" => Expression.GreaterThan(left, right),
                    "gte" => Expression.GreaterThanOrEqual(left, right),
                    "like" => Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, right),
                    "startswith" => Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, right),
                    "endswith" => Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, right),
                    "null" => Expression.Equal(left, Expression.Constant(null)),
                    "notnull" => Expression.NotEqual(left, Expression.Constant(null)),
                    _ => null
                };

                if (expr != null)
                {
                    var lambda = Expression.Lambda<Func<TEntity, bool>>(expr, param);
                    
                    if (predicate == null)
                        predicate = lambda;
                    else
                        predicate = Expression.Lambda<Func<TEntity, bool>>(
                            Expression.AndAlso(predicate.Body, lambda.Body), param);
                }
            }

            return predicate != null ? query.Where(predicate) : query;
        }

        /// <summary>
        /// Aplica filtros em memória
        /// </summary>
        public static IEnumerable<TEntity> ApplyMemoryFilters<TEntity>(
            IEnumerable<TEntity> entities, 
            List<FilterCriteria> filters) where TEntity : class
        {
            if (!filters.Any())
                return entities;

            return entities.Where(entity =>
            {
                foreach (var filter in filters)
                {
                    var prop = typeof(TEntity).GetProperty(filter.Property, 
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    if (prop == null)
                        continue;

                    var value = prop.GetValue(entity);
                    
                    if (!EvaluateMemoryFilter(value, filter.Value, filter.Operator))
                        return false;
                }
                return true;
            });
        }

        /// <summary>
        /// Avalia um filtro em memória
        /// </summary>
        private static bool EvaluateMemoryFilter(object? value, object? filterValue, string operator_)
        {
            return operator_.ToLower() switch
            {
                "in" => EvaluateInOperator(value, filterValue),
                "notin" => !EvaluateInOperator(value, filterValue),
                "between" => EvaluateBetweenOperator(value, filterValue),
                "contains" => EvaluateContainsOperator(value, filterValue),
                _ => false
            };
        }

        /// <summary>
        /// Avalia operador "in"
        /// </summary>
        private static bool EvaluateInOperator(object? value, object? filterValue)
        {
            if (filterValue is IEnumerable<object> enumerable)
                return enumerable.Contains(value);
            return false;
        }

        /// <summary>
        /// Avalia operador "between"
        /// </summary>
        private static bool EvaluateBetweenOperator(object? value, object? filterValue)
        {
            if (filterValue is object[] range && range.Length == 2 && value is IComparable comparable)
            {
                var min = range[0] as IComparable;
                var max = range[1] as IComparable;
                
                if (min != null && max != null)
                {
                    return comparable.CompareTo(min) >= 0 && comparable.CompareTo(max) <= 0;
                }
            }
            return false;
        }

        /// <summary>
        /// Avalia operador "contains"
        /// </summary>
        private static bool EvaluateContainsOperator(object? value, object? filterValue)
        {
            if (value is string str && filterValue is string searchValue)
                return str.Contains(searchValue, StringComparison.OrdinalIgnoreCase);
            return false;
        }

        /// <summary>
        /// Converte valor para o tipo da propriedade
        /// </summary>
        private static object? ConvertValue(object? value, Type targetType)
        {
            if (value == null)
                return null;

            try
            {
                if (targetType.IsAssignableFrom(value.GetType()))
                    return value;

                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                return null;
            }
        }
    }
} 