using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Gerais.Domain.Usuario;

namespace ludusGestao.Provider.Data.Providers.Autenticacao
{
    public class UsuarioAutenticacaoPostgresReadProvider : ReadProviderBase<UsuarioAutenticacao>, IUsuarioAutenticacaoReadProvider
    {
        private readonly LudusGestaoReadDbContext _readContext;

        public UsuarioAutenticacaoPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
            _readContext = context;
        }

        public async Task<UsuarioAutenticacao> ObterPorEmail(string email)
        {
            var usuario = await _readContext.Usuarios
                .FirstOrDefaultAsync(u => u.Email.Endereco == email && u.EstaAtivo());

            if (usuario == null)
                return null;

            // Mapear Usuario para UsuarioAutenticacao
            var usuarioAutenticacao = UsuarioAutenticacao.Criar(usuario.Email.Endereco, usuario.Senha);
            usuarioAutenticacao.AlterarTenant(usuario.TenantId);
            
            return usuarioAutenticacao;
        }

        protected override (IQueryable<UsuarioAutenticacao> Query, int Total) ApplyQueryParams(IQueryable<UsuarioAutenticacao> query, QueryParamsBase queryParams)
        {
            // Implementação básica - pode ser expandida conforme necessário
            var total = query.Count();
            return (query, total);
        }
    }
} 