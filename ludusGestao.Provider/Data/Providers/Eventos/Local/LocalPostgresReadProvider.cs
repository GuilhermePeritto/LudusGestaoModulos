using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LocalEntity = ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Eventos.Local
{
    public class LocalPostgresReadProvider : ILocalReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public LocalPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<LocalEntity?> BuscarPorId(Guid id)
            => await _context.Locais.FirstOrDefaultAsync(l => l.Id == id);

        public async Task<IEnumerable<LocalEntity>> ListarTodos()
            => await _context.Locais.OrderBy(l => l.Nome).ToListAsync();

        public async Task<IEnumerable<LocalEntity>> BuscarPorCapacidade(int capacidadeMinima)
            => await _context.Locais.Where(l => l.Capacidade >= capacidadeMinima).OrderBy(l => l.Capacidade).ToListAsync();

        public async Task<IEnumerable<LocalEntity>> BuscarPorFaixaPreco(decimal valorMinimo, decimal valorMaximo)
            => await Task.FromResult<IEnumerable<LocalEntity>>(new List<LocalEntity>()); // Não implementado

        public async Task<IEnumerable<LocalEntity>> BuscarPorDisponibilidade(bool disponivel)
            => await Task.FromResult<IEnumerable<LocalEntity>>(new List<LocalEntity>()); // Não implementado

        public async Task<bool> ExistePorNome(string nome, Guid? idExcluir = null)
        {
            var query = _context.Locais.Where(l => l.Nome == nome);
            if (idExcluir.HasValue)
                query = query.Where(l => l.Id != idExcluir.Value);
            return await query.AnyAsync();
        }

        public async Task<int> Contar()
            => await _context.Locais.CountAsync();

        public async Task<IEnumerable<LocalEntity>> BuscarPaginado(int pagina, int tamanhoPagina)
            => await _context.Locais.OrderBy(l => l.Nome).Skip((pagina - 1) * tamanhoPagina).Take(tamanhoPagina).ToListAsync();
    }
} 