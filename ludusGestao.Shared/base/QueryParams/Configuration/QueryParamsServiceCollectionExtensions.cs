using Microsoft.Extensions.DependencyInjection;
using LudusGestao.Shared.Domain.QueryParams.Interfaces;
using LudusGestao.Shared.Domain.QueryParams.Implementations;

namespace LudusGestao.Shared.Domain.QueryParams.Configuration
{
    /// <summary>
    /// Extensões para configuração de serviços do QueryParams
    /// </summary>
    public static class QueryParamsServiceCollectionExtensions
    {
        /// <summary>
        /// Adiciona serviços do QueryParams ao container de dependências
        /// </summary>
        public static IServiceCollection AdicionarQueryParams(this IServiceCollection services)
        {
            // Registra as implementações
            services.AddScoped<IValidadorFiltro, ValidadorFiltro>();
            services.AddScoped<IConversorFiltro, ConversorFiltro>();
            services.AddScoped<IProcessadorFiltro, ProcessadorFiltro>();
            services.AddScoped<IProcessadorCampos, ProcessadorCampos>();

            // Registra o processador principal
            services.AddScoped<ProcessadorQueryParams>();

            return services;
        }

        /// <summary>
        /// Adiciona serviços do QueryParams como singletons
        /// </summary>
        public static IServiceCollection AdicionarQueryParamsComoSingleton(this IServiceCollection services)
        {
            // Registra as implementações como singletons
            services.AddSingleton<IValidadorFiltro, ValidadorFiltro>();
            services.AddSingleton<IConversorFiltro, ConversorFiltro>();
            services.AddSingleton<IProcessadorFiltro, ProcessadorFiltro>();
            services.AddSingleton<IProcessadorCampos, ProcessadorCampos>();

            // Registra o processador principal como singleton
            services.AddSingleton<ProcessadorQueryParams>();

            return services;
        }

        /// <summary>
        /// Adiciona serviços do QueryParams como transients
        /// </summary>
        public static IServiceCollection AdicionarQueryParamsComoTransient(this IServiceCollection services)
        {
            // Registra as implementações como transients
            services.AddTransient<IValidadorFiltro, ValidadorFiltro>();
            services.AddTransient<IConversorFiltro, ConversorFiltro>();
            services.AddTransient<IProcessadorFiltro, ProcessadorFiltro>();
            services.AddTransient<IProcessadorCampos, ProcessadorCampos>();

            // Registra o processador principal como transient
            services.AddTransient<ProcessadorQueryParams>();

            return services;
        }

        /// <summary>
        /// Adiciona apenas o processador principal (assumindo que as dependências já estão registradas)
        /// </summary>
        public static IServiceCollection AdicionarApenasProcessadorQueryParams(this IServiceCollection services)
        {
            services.AddScoped<ProcessadorQueryParams>();
            return services;
        }

        /// <summary>
        /// Adiciona apenas o processador principal como singleton
        /// </summary>
        public static IServiceCollection AdicionarApenasProcessadorQueryParamsComoSingleton(this IServiceCollection services)
        {
            services.AddSingleton<ProcessadorQueryParams>();
            return services;
        }

        /// <summary>
        /// Adiciona apenas o processador principal como transient
        /// </summary>
        public static IServiceCollection AdicionarApenasProcessadorQueryParamsComoTransient(this IServiceCollection services)
        {
            services.AddTransient<ProcessadorQueryParams>();
            return services;
        }
    }
} 