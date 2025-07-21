using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Eventos.Domain.Providers;
using ludusGestao.Provider.Data.Providers.Eventos;
using ludusGestao.Provider.Data.Providers;
using ludusGestao.Provider.Data.Configurations;
using System.Reflection;
using System.Linq;

namespace ludusGestao.Provider.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLudusGestaoProvider(
            this IServiceCollection services,
            Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            // Adicionar configuração do banco de dados
            services.AddDatabaseConfiguration(configuration);

            // Registrar providers de Local (apenas Postgres)
            services.AddScoped<LocalPostgresReadProvider>();
            services.AddScoped<LocalPostgresWriteProvider>();

            // Registrar ProviderSelector
            services.AddScoped<ProviderSelector>();

            // Registrar interfaces de provider com factory
            services.AddScoped<ILocalReadProvider>(provider =>
            {
                var selector = provider.GetService<ProviderSelector>()!;
                return selector.GetLocalReadProvider();
            });

            services.AddScoped<ILocalWriteProvider>(provider =>
            {
                var selector = provider.GetService<ProviderSelector>()!;
                return selector.GetLocalWriteProvider();
            });

            // Registrar manualmente os UseCases do fluxo de Local (Eventos)
            services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.CriarLocalUseCase>();
            services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.AtualizarLocalUseCase>();
            services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.RemoverLocalUseCase>();
            services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.BuscarLocalPorIdUseCase>();
            services.AddScoped<ludusGestao.Eventos.Application.UseCases.Local.ListarLocaisUseCase>();

            // Registrar o repositório concreto
            services.AddScoped<ludusGestao.Eventos.Domain.Repositories.ILocalRepository, LocalRepository>();

            // Registrar repositórios do módulo Gerais
            services.AddScoped<ludusGestao.Gerais.Domain.Repositories.IUsuarioRepository, ludusGestao.Provider.Data.Providers.UsuarioRepository>();
            services.AddScoped<ludusGestao.Gerais.Domain.Repositories.IEmpresaRepository, ludusGestao.Provider.Data.Providers.EmpresaRepository>();
            services.AddScoped<ludusGestao.Gerais.Domain.Repositories.IFilialRepository, ludusGestao.Provider.Data.Providers.FilialRepository>();

            return services;
        }

        // Implementação segura do scan automático de serviços
        public static IServiceCollection AddScopedFromAssembliesOf<T1, T2>(this IServiceCollection services)
        {
            var assemblies = new[] { typeof(T1).Assembly, typeof(T2).Assembly };
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic)
                    .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("UseCase"))
                    .Where(t => t.GetInterfaces().Any()) // só classes que implementam interfaces
                    .ToList();
                foreach (var type in types)
                {
                    var serviceType = type.GetInterfaces().First();
                    services.AddScoped(serviceType, type);
                }
            }
            return services;
        }
    }
} 