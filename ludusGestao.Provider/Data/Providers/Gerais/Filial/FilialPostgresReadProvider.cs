using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilialEntity = ludusGestao.Gerais.Domain.Entities.Filial;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.Filial
{
    public class FilialPostgresReadProvider : ProviderBase<FilialEntity>, IFilialReadProvider
    {
        public FilialPostgresReadProvider(LudusGestaoReadDbContext context) : base(context) { }

        public async Task<FilialEntity> BuscarPorId(Guid id)
            => await _dbSet.FirstOrDefaultAsync(f => f.Id == id);

        public async Task<IEnumerable<FilialEntity>> ListarTodos()
            => await _dbSet.OrderBy(f => f.Nome).ToListAsync();

        public async Task<bool> ExistePorCodigo(string codigo)
            => await _dbSet.AnyAsync(f => f.Codigo == codigo);

        public async Task<(IEnumerable<FilialEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_dbSet.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }
    }
} 