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
    public class FilialPostgresReadProvider : IFilialReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public FilialPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<FilialEntity> BuscarPorId(Guid id)
            => await _context.Filiais.FirstOrDefaultAsync(f => f.Id == id);

        public async Task<IEnumerable<FilialEntity>> ListarTodos()
            => await _context.Filiais.OrderBy(f => f.Nome).ToListAsync();

        public async Task<bool> ExistePorCodigo(string codigo)
            => await _context.Filiais.AnyAsync(f => f.Codigo == codigo);

        public async Task<(IEnumerable<FilialEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_context.Filiais.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }

        // Supondo que ApplyQueryParams está disponível (pode ser movido para cá se necessário)
        private (IQueryable<FilialEntity> Query, int Total) ApplyQueryParams(IQueryable<FilialEntity> query, QueryParamsBase queryParams)
        {
            // Implementação fictícia, ajuste conforme sua lógica
            int total = query.Count();
            return (query, total);
        }
    }
} 