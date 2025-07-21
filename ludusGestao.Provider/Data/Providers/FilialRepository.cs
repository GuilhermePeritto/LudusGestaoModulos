using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Provider.Data.Providers
{
    public class FilialRepository : IFilialRepository, LudusGestao.Shared.Domain.Repositories.IRepository<Filial>
    {
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext _readContext;
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext _writeContext;

        public FilialRepository(
            ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext readContext,
            ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext writeContext)
        {
            _readContext = readContext;
            _writeContext = writeContext;
        }

        public async Task Adicionar(Filial filial)
        {
            await _writeContext.Filiais.AddAsync(filial);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<Filial> BuscarPorId(Guid id)
        {
            return await _readContext.Filiais.FindAsync(id);
        }

        public async Task<Filial> BuscarPorId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return null;
            return await BuscarPorId(guid);
        }

        public async Task Atualizar(Filial filial)
        {
            _writeContext.Filiais.Update(filial);
            await _writeContext.SaveChangesAsync();
        }

        public async Task Remover(Filial filial)
        {
            _writeContext.Filiais.Remove(filial);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Filial>> ListarTodos()
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Filiais);
        }

        public async Task<bool> ExistePorCodigo(string codigo)
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Filiais.Any(f => f.Codigo == codigo));
        }
    }
} 