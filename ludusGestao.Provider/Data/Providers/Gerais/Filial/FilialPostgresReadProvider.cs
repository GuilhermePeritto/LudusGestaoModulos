using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.FilialProvider
{
    public class FilialPostgresReadProvider : ReadProviderBase<ludusGestao.Gerais.Domain.Filial.Filial>, IFilialReadProvider
    {
        public FilialPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        protected override (IQueryable<ludusGestao.Gerais.Domain.Filial.Filial> Query, int Total) ApplyQueryParams(IQueryable<ludusGestao.Gerais.Domain.Filial.Filial> query, QueryParamsBase queryParams)
        {
            // Implementação básica - pode ser expandida conforme necessário
            var total = query.Count();
            return (query, total);
        }
    }
} 