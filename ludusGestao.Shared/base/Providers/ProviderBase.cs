using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using System.Reflection;

namespace LudusGestao.Shared.Domain.Providers
{
    public abstract class ProviderBase<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly ProcessadorQueryParams _processadorQueryParams;

        protected ProviderBase(DbContext context, ProcessadorQueryParams processadorQueryParams)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _processadorQueryParams = processadorQueryParams;
        }

        public async Task<IEnumerable<TEntity>> Listar(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // Aplica ordenação
            query = ApplySorting(query, queryParams);
            
            // Calcula total antes da paginação
            var total = await query.CountAsync();
            
            // Aplica paginação
            query = ApplyPagination(query, queryParams);
            
            // Executa query no banco (todos os campos)
            var entities = await query.ToListAsync();
            
            // Aplica filtros de memória se necessário
            if (memoryFilters.Any())
            {
                entities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters).ToList();
            }
            
            return entities;
        }

        public async Task<IEnumerable<TEntity>> Listar()
            => await _dbSet.ToListAsync();

        public async Task<TEntity?> Buscar(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // Aplica ordenação
            query = ApplySorting(query, queryParams);
            
            // Executa query no banco (todos os campos)
            var entity = await query.FirstOrDefaultAsync();
            
            // Aplica filtros de memória se necessário
            if (entity != null && memoryFilters.Any())
            {
                var entities = new[] { entity };
                var filteredEntities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters);
                entity = filteredEntities.FirstOrDefault();
            }
            
            return entity;
        }

        public async Task<IEnumerable<object>> ListarComCampos(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // Aplica ordenação
            query = ApplySorting(query, queryParams);
            
            // Calcula total antes da paginação
            var total = await query.CountAsync();
            
            // Aplica paginação
            query = ApplyPagination(query, queryParams);
            
            // Aplica filtro de campos se especificado (SELECT dinâmico)
            var fields = _processadorQueryParams.AplicarCampos<TEntity>(queryParams);
            if (fields.Any())
            {
                // Usa SELECT dinâmico para retornar apenas os campos solicitados
                var dynamicQuery = _processadorQueryParams.AplicarFiltroCamposComoDTO(query, fields);
                var dynamicEntities = await dynamicQuery.ToListAsync();
                return dynamicEntities;
            }
            
            // Executa query no banco (todos os campos)
            var entities = await query.ToListAsync();
            
            // Aplica filtros de memória se necessário
            if (memoryFilters.Any())
            {
                entities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters).ToList();
            }
            
            return entities.Cast<object>();
        }

        public async Task<object?> BuscarComCampos(QueryParamsBase queryParams)
        {
            var (query, memoryFilters) = _processadorQueryParams.AplicarFiltros(_dbSet.AsQueryable(), queryParams);
            
            // Aplica ordenação
            query = ApplySorting(query, queryParams);
            
            // Aplica filtro de campos se especificado (SELECT dinâmico)
            var fields = _processadorQueryParams.AplicarCampos<TEntity>(queryParams);
            if (fields.Any())
            {
                // Usa SELECT dinâmico para retornar apenas os campos solicitados
                var dynamicQuery = _processadorQueryParams.AplicarFiltroCamposComoDTO(query, fields);
                var dynamicEntity = await dynamicQuery.FirstOrDefaultAsync();
                return dynamicEntity;
            }
            
            // Executa query no banco (todos os campos)
            var entity = await query.FirstOrDefaultAsync();
            
            // Aplica filtros de memória se necessário
            if (entity != null && memoryFilters.Any())
            {
                var entities = new[] { entity };
                var filteredEntities = _processadorQueryParams.AplicarFiltrosMemoria(entities, memoryFilters);
                entity = filteredEntities.FirstOrDefault();
            }
            
            return entity;
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
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                
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
            var page = Math.Max(1, queryParams.Page);
            var limit = Math.Max(1, Math.Min(100, queryParams.Limit)); // Limita a 100 registros por página
            var start = Math.Max(0, queryParams.Start);

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

        protected (IQueryable<TEntity> Query, int Total) ApplyQueryParams(IQueryable<TEntity> query, QueryParamsBase queryParams)
        {
            var (processedQuery, memoryFilters) = _processadorQueryParams.AplicarFiltros(query, queryParams);
            var sortedQuery = ApplySorting(processedQuery, queryParams);
            var total = sortedQuery.Count();
            var paginatedQuery = ApplyPagination(sortedQuery, queryParams);
            
            return (paginatedQuery, total);
        }
    }
} 