using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.FilialProvider
{
    public class FilialPostgresReadProvider : ReadProviderBase<ludusGestao.Gerais.Domain.Filial.Filial>, IFilialReadProvider
    {
        public FilialPostgresReadProvider(LudusGestaoReadDbContext context, ProcessadorQueryParams processadorQueryParams) : base(context, processadorQueryParams)
        {
        }

        // ✅ Provider limpo: não precisa sobrescrever ApplyQueryParams
        // O ReadProviderBase já faz tudo automaticamente
        // Se precisar de filtros específicos da Filial, pode sobrescrever ApplyQueryParams aqui
    }
} 