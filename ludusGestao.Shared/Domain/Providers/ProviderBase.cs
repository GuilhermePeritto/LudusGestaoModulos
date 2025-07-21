using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Shared.Domain.Common;
using System.Reflection;
using System.Linq.Expressions;
using System.Text.Json;

namespace LudusGestao.Shared.Domain.Providers
{
    public abstract class ProviderBase<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected ProviderBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected (IQueryable<TEntity> Query, int Total) ApplyQueryParams(IQueryable<TEntity> query, QueryParamsBase queryParams)
        {
            // Filtros complexos (FilterObjects)
            if (queryParams.FilterObjects != null && queryParams.FilterObjects.Any())
            {
                Expression<Func<TEntity, bool>>? predicate = null;
                var param = Expression.Parameter(typeof(TEntity), "e");
                foreach (var filter in queryParams.FilterObjects)
                {
                    var prop = typeof(TEntity).GetProperty(filter.Property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null)
                        throw new ArgumentException($"Propriedade '{filter.Property}' não existe em {typeof(TEntity).Name}.");
                    var left = Expression.Property(param, prop);
                    var op = filter.Operator?.ToLower() ?? "eq";
                    Expression right = null;
                    Expression expr = null;
                    object value = filter.Value;
                    // Conversão de valor para o tipo correto
                    if (value is JsonElement je)
                        value = JsonElementToObject(je, prop.PropertyType);
                    else if (value is not null && value.GetType() != prop.PropertyType)
                        value = Convert.ChangeType(value, prop.PropertyType);
                    switch (op)
                    {
                        case "eq": expr = Expression.Equal(left, Expression.Constant(value)); break;
                        case "neq": expr = Expression.NotEqual(left, Expression.Constant(value)); break;
                        case "lt": expr = Expression.LessThan(left, Expression.Constant(value)); break;
                        case "lte": expr = Expression.LessThanOrEqual(left, Expression.Constant(value)); break;
                        case "gt": expr = Expression.GreaterThan(left, Expression.Constant(value)); break;
                        case "gte": expr = Expression.GreaterThanOrEqual(left, Expression.Constant(value)); break;
                        case "like":
                            if (prop.PropertyType != typeof(string)) throw new ArgumentException($"LIKE só pode ser usado em string.");
                            expr = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) })!, Expression.Constant(value));
                            break;
                        case "startswith":
                            if (prop.PropertyType != typeof(string)) throw new ArgumentException($"startsWith só pode ser usado em string.");
                            expr = Expression.Call(left, typeof(string).GetMethod("StartsWith", new[] { typeof(string) })!, Expression.Constant(value));
                            break;
                        case "endswith":
                            if (prop.PropertyType != typeof(string)) throw new ArgumentException($"endsWith só pode ser usado em string.");
                            expr = Expression.Call(left, typeof(string).GetMethod("EndsWith", new[] { typeof(string) })!, Expression.Constant(value));
                            break;
                        case "in":
                            var enumerableType = typeof(IEnumerable<>).MakeGenericType(prop.PropertyType);
                            var containsMethod = enumerableType.GetMethod("Contains", new[] { prop.PropertyType });
                            expr = Expression.Call(Expression.Constant(value), containsMethod!, left);
                            break;
                        case "notin":
                            var enumerableType2 = typeof(IEnumerable<>).MakeGenericType(prop.PropertyType);
                            var containsMethod2 = enumerableType2.GetMethod("Contains", new[] { prop.PropertyType });
                            expr = Expression.Not(Expression.Call(Expression.Constant(value), containsMethod2!, left));
                            break;
                        case "null": expr = Expression.Equal(left, Expression.Constant(null)); break;
                        case "notnull": expr = Expression.NotEqual(left, Expression.Constant(null)); break;
                        case "between":
                            if (value is not System.Text.Json.JsonElement arr || arr.ValueKind != JsonValueKind.Array || arr.GetArrayLength() != 2)
                                throw new ArgumentException("between espera um array de 2 valores");
                            var min = JsonElementToObject(arr[0], prop.PropertyType);
                            var max = JsonElementToObject(arr[1], prop.PropertyType);
                            expr = Expression.AndAlso(
                                Expression.GreaterThanOrEqual(left, Expression.Constant(min)),
                                Expression.LessThanOrEqual(left, Expression.Constant(max)));
                            break;
                        default:
                            throw new ArgumentException($"Operador '{op}' não suportado.");
                    }
                    if (predicate == null)
                        predicate = Expression.Lambda<Func<TEntity, bool>>(expr, param);
                    else
                        predicate = filter.And
                            ? Expression.Lambda<Func<TEntity, bool>>(Expression.AndAlso(predicate.Body, expr), param)
                            : Expression.Lambda<Func<TEntity, bool>>(Expression.OrElse(predicate.Body, expr), param);
                }
                query = query.Where(predicate!);
            }
            // Filtro simples (string)
            else if (!string.IsNullOrWhiteSpace(queryParams.Filter))
            {
                // Espera-se: filter=Propriedade:valor
                var parts = queryParams.Filter.Split(':', 2);
                if (parts.Length != 2)
                    throw new ArgumentException("Filtro deve estar no formato 'Propriedade:valor'.");
                var propName = parts[0];
                var value = parts[1];
                var prop = typeof(TEntity).GetProperty(propName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                    throw new ArgumentException($"Propriedade '{propName}' não existe em {typeof(TEntity).Name}.");
                if (prop.PropertyType == typeof(string))
                    query = query.Where(e => EF.Functions.Like(EF.Property<string>(e, prop.Name), $"%{value}%"));
                else if (prop.PropertyType == typeof(int) && int.TryParse(value, out var intVal))
                    query = query.Where(e => EF.Property<int>(e, prop.Name) == intVal);
                else if (prop.PropertyType == typeof(Guid) && Guid.TryParse(value, out var guidVal))
                    query = query.Where(e => EF.Property<Guid>(e, prop.Name) == guidVal);
                else
                    throw new ArgumentException($"Filtro para tipo '{prop.PropertyType.Name}' não suportado.");
            }
            var total = query.Count();
            // Ordenação
            if (!string.IsNullOrWhiteSpace(queryParams.Sort))
            {
                var prop = typeof(TEntity).GetProperty(queryParams.Sort.TrimStart('-'), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                    throw new ArgumentException($"Propriedade de ordenação '{queryParams.Sort}' não existe em {typeof(TEntity).Name}.");
                if (queryParams.Sort.StartsWith("-"))
                    query = query.OrderByDescending(e => EF.Property<object>(e, prop.Name));
                else
                    query = query.OrderBy(e => EF.Property<object>(e, prop.Name));
            }
            else
            {
                var idProp = typeof(TEntity).GetProperty("Id");
                if (idProp != null)
                    query = query.OrderBy(e => EF.Property<object>(e, "Id"));
            }
            // Paginação
            query = query.Skip((queryParams.Page - 1) * queryParams.Limit).Take(queryParams.Limit);
            return (query, total);
        }

        private object JsonElementToObject(JsonElement je, Type type)
        {
            if (type == typeof(string)) return je.GetString();
            if (type == typeof(int)) return je.GetInt32();
            if (type == typeof(Guid)) return je.GetGuid();
            if (type == typeof(bool)) return je.GetBoolean();
            if (type == typeof(decimal)) return je.GetDecimal();
            if (type == typeof(double)) return je.GetDouble();
            if (type == typeof(DateTime)) return je.GetDateTime();
            if (type.IsEnum) return Enum.Parse(type, je.GetString()!);
            if (je.ValueKind == JsonValueKind.Array)
            {
                var listType = typeof(List<>).MakeGenericType(type);
                var list = (System.Collections.IList)Activator.CreateInstance(listType)!;
                foreach (var item in je.EnumerateArray())
                    list.Add(JsonElementToObject(item, type.GetElementType() ?? type.GetGenericArguments().FirstOrDefault() ?? typeof(object)));
                return list;
            }
            throw new ArgumentException($"Conversão de JsonElement para {type.Name} não suportada.");
        }
    }
} 