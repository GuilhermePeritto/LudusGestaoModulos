using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Eventos.Domain.Local.Interfaces;
using ludusGestao.Provider.Data.Providers;
using LudusGestao.Shared.Domain.QueryParams.Configuration;

namespace ludusGestao.Provider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderModule(this IServiceCollection services)
        {
            // Registra os componentes do QueryParams
            services.AdicionarQueryParams();
            
            // Registra os providers
            ProviderSelector.RegistrarProviders(services);
            return services;
        }
    }
} 