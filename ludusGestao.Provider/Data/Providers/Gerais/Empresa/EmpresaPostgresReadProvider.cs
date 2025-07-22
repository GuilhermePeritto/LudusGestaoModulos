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
    public class EmpresaPostgresReadProvider : IEmpresaReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public EmpresaPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<EmpresaEntity> BuscarPorId(Guid id)
            => await _context.Empresas.FirstOrDefaultAsync(e => e.Id == id);

        public async Task<IEnumerable<EmpresaEntity>> ListarTodos()
            => await _context.Empresas.OrderBy(e => e.Nome).ToListAsync();

        public async Task<bool> ExistePorCnpj(string cnpj)
            => await _context.Empresas.AnyAsync(e => e.Cnpj.Valor == cnpj);

        public async Task<(IEnumerable<EmpresaEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_context.Empresas.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }

        // Supondo que ApplyQueryParams está disponível (pode ser movido para cá se necessário)
        private (IQueryable<EmpresaEntity> Query, int Total) ApplyQueryParams(IQueryable<EmpresaEntity> query, QueryParamsBase queryParams)
        {
            // Implementação fictícia, ajuste conforme sua lógica
            int total = query.Count();
            return (query, total);
        }
    }
} 