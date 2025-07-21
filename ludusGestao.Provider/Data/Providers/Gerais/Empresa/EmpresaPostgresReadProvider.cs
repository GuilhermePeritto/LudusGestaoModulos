using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmpresaEntity = ludusGestao.Gerais.Domain.Entities.Empresa;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.Empresa
{
    public class EmpresaPostgresReadProvider : ProviderBase<EmpresaEntity>, IEmpresaReadProvider
    {
        public EmpresaPostgresReadProvider(LudusGestaoReadDbContext context) : base(context) { }

        public async Task<EmpresaEntity> BuscarPorId(Guid id)
            => await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<EmpresaEntity>> ListarTodos()
            => await _dbSet.OrderBy(e => e.Nome).ToListAsync();

        public async Task<bool> ExistePorCnpj(string cnpj)
            => await _dbSet.AnyAsync(e => e.Cnpj.Valor == cnpj);

        public async Task<(IEnumerable<EmpresaEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_dbSet.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }
    }
} 