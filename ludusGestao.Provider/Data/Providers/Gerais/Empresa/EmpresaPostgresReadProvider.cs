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
        public EmpresaPostgresReadProvider(LudusGestaoReadDbContext context, ProcessadorQueryParams processadorQueryParams) : base(context, processadorQueryParams)
        {
        }

        // ✅ Provider limpo: não precisa sobrescrever nada
        // O ReadProviderBase já faz tudo automaticamente
        // Se precisar de filtros específicos da Empresa, pode sobrescrever ApplyQueryParams aqui
    }
} 