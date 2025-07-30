using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;
using ludusGestao.Eventos.Domain.Local.Interfaces;

namespace ludusGestao.Provider.Data.Providers.Eventos.LocalProvider
{
    public class LocalPostgresReadProvider : ReadProviderBase<ludusGestao.Eventos.Domain.Local.Local>, ILocalReadProvider
    {
        public LocalPostgresReadProvider(LudusGestaoReadDbContext context, ProcessadorQueryParams processadorQueryParams) : base(context, processadorQueryParams)
        {
        }

        // ✅ Provider limpo: não precisa sobrescrever ApplyQueryParams
        // O ReadProviderBase já faz tudo automaticamente
        // Se precisar de filtros específicos do Local, pode sobrescrever ApplyQueryParams aqui
    }
}