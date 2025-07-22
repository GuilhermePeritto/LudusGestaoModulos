using ludusGestao.Provider.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Autenticacao.Domain.Entities;
using ludusGestao.Autenticacao.Domain.Providers;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Provider.Data.Providers.Autenticacao
{
    public class UsuarioAutenticacaoPostgresReadProvider : IUsuarioAutenticacaoReadProvider
    {
        private readonly LudusGestaoReadDbContext _context;
        public UsuarioAutenticacaoPostgresReadProvider(LudusGestaoReadDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioAutenticacao> ObterPorEmail(Email email)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Email.Valor == email.Valor)
                .Select(u => new UsuarioAutenticacao
                {
                    Id = u.Id,
                    Email = u.Email.Valor,
                    Senha = u.Senha,
                    TenantId = u.TenantId,
                    Ativo = u.Situacao == ludusGestao.Gerais.Domain.Enums.SituacaoUsuario.Ativo
                })
                .FirstOrDefaultAsync();
            return usuario;
        }
    }
} 