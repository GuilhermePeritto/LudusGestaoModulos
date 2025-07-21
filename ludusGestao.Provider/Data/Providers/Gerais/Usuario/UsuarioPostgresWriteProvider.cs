using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsuarioEntity = ludusGestao.Gerais.Domain.Entities.Usuario;
using ludusGestao.Gerais.Domain.Providers;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Providers.Gerais.Usuario
{
    public class UsuarioPostgresWriteProvider : IUsuarioWriteProvider
    {
        private readonly LudusGestaoWriteDbContext _context;
        public UsuarioPostgresWriteProvider(LudusGestaoWriteDbContext context)
        {
            _context = context;
        }

        public async Task Adicionar(UsuarioEntity usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
        }

        public async Task Atualizar(UsuarioEntity usuario)
        {
            _context.Usuarios.Update(usuario);
        }

        public async Task Remover(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
                _context.Usuarios.Remove(usuario);
        }

        public async Task<int> SalvarAlteracoes()
        {
            return await _context.SaveChangesAsync();
        }
    }
} 