using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.FilialProvider
{
    public class FilialPostgresWriteProvider : WriteProviderBase<ludusGestao.Gerais.Domain.Filial.Filial>, IFilialWriteProvider
    {
        public FilialPostgresWriteProvider(LudusGestaoWriteDbContext context) : base(context)
        {
        }
    }
} 