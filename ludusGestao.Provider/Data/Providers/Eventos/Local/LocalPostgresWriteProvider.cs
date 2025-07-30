using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Eventos.LocalProvider
{
    public class LocalPostgresWriteProvider : WriteProviderBase<ludusGestao.Eventos.Domain.Local.Local>, ILocalWriteProvider
    {
        public LocalPostgresWriteProvider(LudusGestaoWriteDbContext context) : base(context)
        {
        }
    }
} 