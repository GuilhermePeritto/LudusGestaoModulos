using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;

namespace ludusGestao.Provider.Data.Providers.Autenticacao
{
    public class UsuarioAutenticacaoPostgresReadProvider : ProviderBase<UsuarioAutenticacao>, IUsuarioAutenticacaoReadProvider
    {
        public UsuarioAutenticacaoPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        public async Task<UsuarioAutenticacao> ObterPorEmail(string email)
        {
            return await _context.Usuario
                .FirstOrDefaultAsync(u => u.Email == email && u.Ativo);
        }
    }
} 