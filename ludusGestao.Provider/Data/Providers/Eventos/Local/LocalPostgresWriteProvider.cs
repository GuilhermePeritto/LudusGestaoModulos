using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LocalEntity = ludusGestao.Eventos.Domain.Entities.Local;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Eventos.Local
{
    public class LocalPostgresWriteProvider : ILocalWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;
        public LocalPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(LocalEntity local)
        {
            await _context.Locais.AddAsync(local);
        }

        public async Task Atualizar(LocalEntity local)
        {
            _context.Locais.Update(local);
        }

        public async Task Remover(Guid id)
        {
            var local = await _context.Locais.FindAsync(id);
            if (local != null)
                _context.Locais.Remove(local);
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 