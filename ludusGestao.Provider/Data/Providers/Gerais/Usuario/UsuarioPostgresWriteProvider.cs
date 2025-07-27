using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.UsuarioProvider
{
    public class UsuarioPostgresWriteProvider : WriteProviderBase<ludusGestao.Gerais.Domain.Usuario.Usuario>, IUsuarioWriteProvider
    {
        public UsuarioPostgresWriteProvider(LudusGestaoWriteDbContext context) : base(context)
        {
        }
    }
} 