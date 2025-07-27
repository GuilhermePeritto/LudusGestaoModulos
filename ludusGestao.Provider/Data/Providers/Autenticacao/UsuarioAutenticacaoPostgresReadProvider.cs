using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Gerais.Domain.Usuario;
using LudusGestao.Shared.Tenant;
using Microsoft.EntityFrameworkCore.Infrastructure;

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
            // Buscar usuário por email usando a propriedade do Value Object
            var query = _readContext.Usuarios.Where(u => u.Email.Endereco == email);
            
            // Aplicar filtro de tenant se necessário
            var tenantContext = _readContext.GetService<ITenantContext>();
            if (tenantContext != null && !tenantContext.IgnorarFiltroTenant && tenantContext.TenantIdNullable.HasValue)
            {
                query = query.Where(u => u.TenantId == tenantContext.TenantIdNullable.Value);
            }
            
            var usuario = await query.FirstOrDefaultAsync();

            if (usuario == null || !usuario.EstaAtivo())
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