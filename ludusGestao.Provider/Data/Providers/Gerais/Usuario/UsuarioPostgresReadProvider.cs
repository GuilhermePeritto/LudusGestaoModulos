using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.UsuarioProvider
{
    public class UsuarioPostgresReadProvider : ReadProviderBase<ludusGestao.Gerais.Domain.Usuario.Usuario>, IUsuarioReadProvider
    {
        public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        protected override (IQueryable<ludusGestao.Gerais.Domain.Usuario.Usuario> Query, int Total) ApplyQueryParams(IQueryable<ludusGestao.Gerais.Domain.Usuario.Usuario> query, QueryParamsBase queryParams)
        {
            // Implementação básica - pode ser expandida conforme necessário
            var total = query.Count();
            return (query, total);
        }
    }
} 