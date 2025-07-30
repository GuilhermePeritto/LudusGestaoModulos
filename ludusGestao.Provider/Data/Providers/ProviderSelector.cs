using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Provider.Data.Providers.Eventos.LocalProvider;
using ludusGestao.Provider.Data.Providers.Gerais.EmpresaProvider;
using ludusGestao.Provider.Data.Providers.Gerais.FilialProvider;
using ludusGestao.Provider.Data.Providers.Gerais.UsuarioProvider;
using ludusGestao.Provider.Data.Providers.Autenticacao;

namespace ludusGestao.Provider.Data.Providers
{
    public static class ProviderSelector
    {
        public static void RegistrarProviders(IServiceCollection services)
        {
            // Eventos
            services.AddScoped<ILocalReadProvider, LocalPostgresReadProvider>();
            services.AddScoped<ILocalWriteProvider, LocalPostgresWriteProvider>();

            // Gerais
            services.AddScoped<IEmpresaReadProvider, EmpresaPostgresReadProvider>();
            services.AddScoped<IEmpresaWriteProvider, EmpresaPostgresWriteProvider>();
            services.AddScoped<IFilialReadProvider, FilialPostgresReadProvider>();
            services.AddScoped<IFilialWriteProvider, FilialPostgresWriteProvider>();
            services.AddScoped<IUsuarioReadProvider, UsuarioPostgresReadProvider>();
            services.AddScoped<IUsuarioWriteProvider, UsuarioPostgresWriteProvider>();

            // Autenticação
            services.AddScoped<IUsuarioAutenticacaoReadProvider, UsuarioAutenticacaoPostgresReadProvider>();
        }
    }
} 