using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;

namespace ludusGestao.Provider.Data.Providers.Eventos.LocalProvider
{
    public class LocalPostgresReadProvider : ReadProviderBase<ludusGestao.Eventos.Domain.Entities.Local.Local>, ILocalReadProvider
    {
        public LocalPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        protected override (IQueryable<ludusGestao.Eventos.Domain.Entities.Local.Local> Query, int Total) ApplyQueryParams(IQueryable<ludusGestao.Eventos.Domain.Entities.Local.Local> query, QueryParamsBase queryParams)
        {
            // Implementação básica - pode ser expandida conforme necessário
            var total = query.Count();
            return (query, total);
        }
    }
}