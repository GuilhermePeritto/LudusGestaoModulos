using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LudusGestao.Shared.Domain.Providers
{
    public abstract class WriteProviderBase<TEntity> : IWriteProvider<TEntity> where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        protected WriteProviderBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public virtual async Task Remover(Guid id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
                _dbSet.Remove(entity);
        }

        public virtual async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 