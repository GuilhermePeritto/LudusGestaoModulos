using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LudusGestao.Shared.Domain.Entities;

namespace LudusGestao.Shared.Domain.Repositories
{
    public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : EntidadeBase
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> BuscarPorId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return null;
            return await _dbSet.FindAsync(guid);
        }

        public virtual async Task Adicionar(TEntity entidade)
        {
            await _dbSet.AddAsync(entidade);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Atualizar(TEntity entidade)
        {
            _dbSet.Update(entidade);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remover(TEntity entidade)
        {
            _dbSet.Remove(entidade);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> ListarTodos()
        {
            return await _dbSet.ToListAsync();
        }
    }
} 