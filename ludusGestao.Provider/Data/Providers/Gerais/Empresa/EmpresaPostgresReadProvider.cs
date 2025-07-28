using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.EmpresaProvider
{
    public class EmpresaPostgresReadProvider : ReadProviderBase<ludusGestao.Gerais.Domain.Empresa.Empresa>, IEmpresaReadProvider
    {
        public EmpresaPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
        {
        }

        protected override (IQueryable<ludusGestao.Gerais.Domain.Empresa.Empresa> Query, int Total) ApplyQueryParams(IQueryable<ludusGestao.Gerais.Domain.Empresa.Empresa> query, QueryParamsBase queryParams)
        {
            // O processamento de filtros agora é feito automaticamente pelo ReadProviderBase
            // Esta implementação pode ser expandida para filtros específicos da entidade se necessário
            var total = query.Count();
            return (query, total);
        }
    }
} 