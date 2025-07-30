using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Gerais.Domain.Usuario;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Providers;
using LudusGestao.Shared.Domain.QueryParams.Helpers;

namespace ludusGestao.Provider.Data.Providers.Autenticacao
{
    public class UsuarioAutenticacaoPostgresReadProvider : ProviderBase<Usuario>, IUsuarioAutenticacaoReadProvider
    {
        public UsuarioAutenticacaoPostgresReadProvider(
            LudusGestaoReadDbContext context, 
            ProcessadorQueryParams processadorQueryParams) 
            : base(context, processadorQueryParams)
        {
        }

        public async Task<UsuarioAutenticacao?> Buscar(QueryParamsBase queryParams)
        {
            // Usar o método Buscar do ProviderBase que retorna Usuario
            var usuario = await base.Buscar(queryParams) as Usuario;
            return usuario != null ? MapToUsuarioAutenticacao(usuario) : null;
        }

        private UsuarioAutenticacao MapToUsuarioAutenticacao(Usuario usuario)
        {
            var usuarioAutenticacao = UsuarioAutenticacao.Criar(usuario.Email.Endereco, usuario.Senha);
            usuarioAutenticacao.AlterarTenant(usuario.TenantId);
            
            // Usar reflection para definir o ID
            var idProperty = typeof(UsuarioAutenticacao).GetProperty("Id");
            if (idProperty != null)
            {
                idProperty.SetValue(usuarioAutenticacao, usuario.Id);
            }

            // Usar reflection para definir o status ativo
            var ativoProperty = typeof(UsuarioAutenticacao).GetProperty("Ativo");
            if (ativoProperty != null)
            {
                ativoProperty.SetValue(usuarioAutenticacao, usuario.EstaAtivo());
            }

            return usuarioAutenticacao;
        }
    }
} 