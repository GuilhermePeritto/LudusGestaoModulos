using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;

namespace LudusGestao.Shared.Domain.Providers
{
    public abstract class ReadProviderBase<TEntity> : IReadProvider<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ProcessadorQueryParams _processadorQueryParams;

        protected ReadProviderBase(DbContext context, ProcessadorQueryParams processadorQueryParams)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _processadorQueryParams = processadorQueryParams;
        }

        public virtual async Task<IEnumerable<TEntity>> Listar()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<object>> Listar(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // ✅ Aplica filtros específicos da entidade (se implementado)
            var (filteredQuery, total) = ApplyQueryParams(query, queryParams);
            
            // Aplica ordenação
            filteredQuery = ApplySorting(filteredQuery, queryParams);
            
            // Aplica paginação
            filteredQuery = ApplyPagination(filteredQuery, queryParams);
            
            // Aplica filtro de campos se especificado
            var fields = _processadorQueryParams.AplicarCampos<TEntity>(queryParams);
            Console.WriteLine($"DEBUG: Fields processados: {string.Join(", ", fields)}");
            Console.WriteLine($"DEBUG: QueryParams.Fields original: {queryParams.Fields}");
            
            // Executa query no banco (todos os campos)
            var entities = await filteredQuery.ToListAsync();
            
            // Aplica filtros de memória se necessário
            if (memoryFilters.Any())
            {
                entities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters).ToList();
            }
            
            // Aplica filtro de campos se especificado
            if (fields.Any())
            {
                var filteredEntities = _processadorQueryParams.AplicarFiltroCampos(entities, fields);
                return filteredEntities;
            }
            
            return entities.Cast<object>();
        }

        public virtual async Task<TEntity?> Buscar(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // ✅ Aplica filtros específicos da entidade (se implementado)
            var (filteredQuery, total) = ApplyQueryParams(query, queryParams);
            
            // Aplica ordenação
            filteredQuery = ApplySorting(filteredQuery, queryParams);
            
            // Executa query no banco (todos os campos) - sempre retorna entidade completa
            var entity = await filteredQuery.FirstOrDefaultAsync();
            
            // Aplica filtros de memória se necessário
            if (entity != null && memoryFilters.Any())
            {
                var entities = new[] { entity };
                var filteredEntities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters);
                entity = filteredEntities.FirstOrDefault();
            }
            
            return entity;
        }

        // ✅ Método virtual com implementação padrão (não mais abstrato)
        protected virtual (IQueryable<TEntity> Query, int Total) ApplyQueryParams(IQueryable<TEntity> query, QueryParamsBase queryParams)
        {
            // ✅ Implementação padrão: não aplica filtros específicos
            // Cada provider pode sobrescrever para adicionar filtros customizados
            var total = query.Count();
            return (query, total);
        }

        private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, QueryParamsBase queryParams)
        {
            if (string.IsNullOrWhiteSpace(queryParams.Sort))
                return query;

            var sortParts = queryParams.Sort.Split(',', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var sortPart in sortParts)
            {
                var parts = sortPart.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) continue;

                var propertyName = parts[0];
                var isDescending = parts.Length > 1 && parts[1].ToLower() == "desc";
                
                var prop = typeof(TEntity).GetProperty(propertyName, 
                    System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                
                if (prop == null) continue;

                if (isDescending)
                {
                    query = query.OrderByDescending(e => EF.Property<object>(e, prop.Name));
                }
                else
                {
                    query = query.OrderBy(e => EF.Property<object>(e, prop.Name));
                }
            }

            return query;
        }

        private IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, QueryParamsBase queryParams)
        {
            var page = System.Math.Max(1, queryParams.Page);
            var limit = System.Math.Max(1, System.Math.Min(100, queryParams.Limit)); // Limita a 100 registros por página
            var start = System.Math.Max(0, queryParams.Start);

            if (page > 1)
            {
                query = query.Skip((page - 1) * limit);
            }
            else if (start > 0)
            {
                query = query.Skip(start);
            }

            return query.Take(limit);
        }
    }
} 