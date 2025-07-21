using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsuarioEntity = ludusGestao.Gerais.Domain.Entities.Usuario;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.Usuario
{
    public class UsuarioPostgresReadProvider : ProviderBase<UsuarioEntity>, IUsuarioReadProvider
    {
        public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context) : base(context) { }

        public async Task<UsuarioEntity> BuscarPorId(Guid id)
            => await _dbSet.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<IEnumerable<UsuarioEntity>> ListarTodos()
            => await _dbSet.OrderBy(u => u.Nome).ToListAsync();

        public async Task<bool> ExistePorEmail(string email)
            => await _dbSet.AnyAsync(u => u.Email.Valor == email);

        public async Task<(IEnumerable<UsuarioEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_dbSet.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }
    }
} 