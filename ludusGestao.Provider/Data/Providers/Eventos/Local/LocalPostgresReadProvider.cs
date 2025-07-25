using Microsoft.EntityFrameworkCore;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

public class LocalPostgresReadProvider : ProviderBase<Local>, ILocalReadProvider
{
    public LocalPostgresReadProvider(LudusGestaoReadDbContext context) : base(context)
    {

    }
}