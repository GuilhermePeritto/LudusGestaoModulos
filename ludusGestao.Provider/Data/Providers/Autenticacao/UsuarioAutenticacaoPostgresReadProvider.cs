using ludusGestao.Provider.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Autenticacao.Domain.Entities;
using ludusGestao.Autenticacao.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Autenticacao
{
    public class UsuarioAutenticacaoPostgresReadProvider : IUsuarioAutenticacaoReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public UsuarioAutenticacaoPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioAutenticacao> ObterPorLogin(string login)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email.Valor == login)
                .Select(u => new UsuarioAutenticacao
                {
                    Id = u.Id,
                    Login = u.Email.Valor,
                    Senha = u.Senha,
                    TenantId = u.TenantId,
                    Ativo = u.Situacao == ludusGestao.Gerais.Domain.Enums.SituacaoUsuario.Ativo
                })
                .FirstOrDefaultAsync();
            return usuario;
        }
    }
} 