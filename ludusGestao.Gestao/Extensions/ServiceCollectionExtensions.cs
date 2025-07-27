using ludusGestao.Gestao.Domain.Entities.Cliente.Interfaces;
using ludusGestao.Gestao.Domain.Entities.Cliente.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace ludusGestao.Gestao.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGestaoServices(this IServiceCollection services)
        {
            // Use Cases
            services.AddScoped<ICriarClienteUseCase, CriarClienteUseCase>();

            return services;
        }
    }
} 