using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities;
using ludusGestao.Eventos.Domain.Repositories;
using ludusGestao.Eventos.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Eventos
{
    public class LocalRepository : ILocalRepository, LudusGestao.Shared.Domain.Repositories.IRepository<Local>
    {
        private readonly ILocalReadProvider _readProvider;
        private readonly ILocalWriteProvider _writeProvider;

        public LocalRepository(ILocalReadProvider readProvider, ILocalWriteProvider writeProvider)
        {
            _readProvider = readProvider;
            _writeProvider = writeProvider;
        }

        public async Task Adicionar(Local local)
        {
            await _writeProvider.Adicionar(local);
            await _writeProvider.SalvarAlteracoes();
        }

        public async Task<Local> BuscarPorId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return null;
            return await _readProvider.BuscarPorId(guid);
        }

        public async Task Atualizar(Local local)
        {
            await _writeProvider.Atualizar(local);
            await _writeProvider.SalvarAlteracoes();
        }

        public async Task Remover(Local local)
        {
            await _writeProvider.Remover(local.Id);
            await _writeProvider.SalvarAlteracoes();
        }

        public async Task<IEnumerable<Local>> ListarTodos()
        {
            return await _readProvider.ListarTodos();
        }

        public async Task<bool> ExistePorNome(string nome)
        {
            return await _readProvider.ExistePorNome(nome);
        }
    }
} 