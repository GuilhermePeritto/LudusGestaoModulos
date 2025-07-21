using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Entities;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Eventos
{
    public class LocalPostgresWriteProvider : ILocalWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;

        public LocalPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(Local local)
        {
            await _context.Locais.AddAsync(local);
        }

        public async Task Atualizar(Local local)
        {
            var localExistente = await _context.Locais.FindAsync(local.Id);
            if (localExistente != null)
            {
                _context.Entry(localExistente).CurrentValues.SetValues(local);
            }
        }

        public async Task Remover(Guid id)
        {
            var local = await _context.Locais.FindAsync(id);
            if (local != null)
            {
                _context.Locais.Remove(local);
            }
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 