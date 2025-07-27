using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Eventos.Domain.Entities.Local.Interfaces;
using ludusGestao.Provider.Data.Providers;

namespace ludusGestao.Provider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProviderModule(this IServiceCollection services)
        {
            ProviderSelector.RegistrarProviders(services);
            return services;
        }
    }
} 