using System.Linq;
using Microsoft.EntityFrameworkCore;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Provider.Data.Contexts;
using LudusGestao.Shared.Domain.QueryParams;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Domain.Providers;

namespace ludusGestao.Provider.Data.Providers.Gerais.UsuarioProvider
{
    public class UsuarioPostgresReadProvider : ReadProviderBase<ludusGestao.Gerais.Domain.Usuario.Usuario>, IUsuarioReadProvider
    {
        public UsuarioPostgresReadProvider(LudusGestaoReadDbContext context, ProcessadorQueryParams processadorQueryParams) : base(context, processadorQueryParams)
        {
        }

        // ✅ Provider limpo: não precisa sobrescrever ApplyQueryParams
        // O ReadProviderBase já faz tudo automaticamente
        // Se precisar de filtros específicos do Usuario, pode sobrescrever ApplyQueryParams aqui
    }
} 