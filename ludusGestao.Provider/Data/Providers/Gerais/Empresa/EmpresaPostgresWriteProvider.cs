using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.EmpresaProvider
{
    public class EmpresaPostgresWriteProvider : WriteProviderBase<ludusGestao.Gerais.Domain.Empresa.Empresa>, IEmpresaWriteProvider
    {
        public EmpresaPostgresWriteProvider(LudusGestaoWriteDbContext context) : base(context)
        {
        }
    }
} 