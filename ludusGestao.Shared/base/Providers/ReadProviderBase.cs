using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Shared.Domain.Common;

namespace LudusGestao.Shared.Domain.Providers
{
    public abstract class ReadProviderBase<TEntity> : IReadProvider<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected ReadProviderBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<IEnumerable<TEntity>> Listar()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> Listar(QueryParamsBase queryParams)
        {
            var (query, _) = ApplyQueryParams(_dbSet.AsQueryable(), queryParams);
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity> Buscar(QueryParamsBase queryParams)
        {
            var (query, _) = ApplyQueryParams(_dbSet.AsQueryable(), queryParams);
            return await query.FirstOrDefaultAsync();
        }

        protected abstract (IQueryable<TEntity> Query, int Total) ApplyQueryParams(IQueryable<TEntity> query, QueryParamsBase queryParams);
    }
} 