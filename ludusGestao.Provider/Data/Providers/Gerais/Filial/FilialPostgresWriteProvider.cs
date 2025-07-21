using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FilialEntity = ludusGestao.Gerais.Domain.Entities.Filial;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Gerais.Filial
{
    public class FilialPostgresWriteProvider : IFilialWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;
        public FilialPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(FilialEntity filial)
        {
            await _context.Filiais.AddAsync(filial);
        }

        public async Task Atualizar(FilialEntity filial)
        {
            _context.Filiais.Update(filial);
        }

        public async Task Remover(Guid id)
        {
            var filial = await _context.Filiais.FindAsync(id);
            if (filial != null)
                _context.Filiais.Remove(filial);
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 