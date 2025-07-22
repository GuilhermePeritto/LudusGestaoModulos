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
    public class UsuarioPostgresReadProvider : IUsuarioReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioEntity> BuscarPorId(Guid id)
            => await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        public async Task<IEnumerable<UsuarioEntity>> ListarTodos()
            => await _context.Usuarios.OrderBy(u => u.Nome).ToListAsync();

        public async Task<bool> ExistePorEmail(string email)
            => await _context.Usuarios.AnyAsync(u => u.Email.Valor == email);

        public async Task<(IEnumerable<UsuarioEntity> Itens, int Total)> ListarPaginado(QueryParamsBase query)
        {
            var (q, total) = ApplyQueryParams(_context.Usuarios.AsQueryable(), query);
            return (await q.ToListAsync(), total);
        }

        // Supondo que ApplyQueryParams está disponível (pode ser movido para cá se necessário)
        private (IQueryable<UsuarioEntity> Query, int Total) ApplyQueryParams(IQueryable<UsuarioEntity> query, QueryParamsBase queryParams)
        {
            // Implementação fictícia, ajuste conforme sua lógica
            int total = query.Count();
            return (query, total);
        }
    }
} 