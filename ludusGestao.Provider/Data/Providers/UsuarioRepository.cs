using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Gerais.Domain.Entities;
using ludusGestao.Gerais.Domain.Repositories;

namespace ludusGestao.Provider.Data.Providers
{
    public class UsuarioRepository : IUsuarioRepository, LudusGestao.Shared.Domain.Repositories.IRepository<Usuario>
    {
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext _readContext;
        private readonly ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext _writeContext;

        public UsuarioRepository(
            ludusGestao.Provider.Data.Contexts.LudusGestaoReadDbContext readContext,
            ludusGestao.Provider.Data.Contexts.LudusGestaoWriteDbContext writeContext)
        {
            _readContext = readContext;
            _writeContext = writeContext;
        }

        public async Task Adicionar(Usuario usuario)
        {
            await _writeContext.Usuarios.AddAsync(usuario);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<Usuario> BuscarPorId(string id)
        {
            if (!Guid.TryParse(id, out var guid))
                return null;
            return await BuscarPorId(guid);
        }

        public async Task<Usuario> BuscarPorId(Guid id)
        {
            return await _readContext.Usuarios.FindAsync(id);
        }

        public async Task Atualizar(Usuario usuario)
        {
            _writeContext.Usuarios.Update(usuario);
            await _writeContext.SaveChangesAsync();
        }

        public async Task Remover(Usuario usuario)
        {
            _writeContext.Usuarios.Remove(usuario);
            await _writeContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Usuario>> ListarTodos()
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Usuarios);
        }

        public async Task<bool> ExistePorEmail(string email)
        {
            return await System.Threading.Tasks.Task.FromResult(_readContext.Usuarios.Any(u => u.Email.Valor == email));
        }
    }
} 