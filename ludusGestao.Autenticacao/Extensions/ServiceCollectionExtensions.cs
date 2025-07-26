using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.UseCases;
using ludusGestao.Autenticacao.Application.Services;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Validations;

namespace ludusGestao.Autenticacao.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAutenticacaoModule(this IServiceCollection services)
        {
            // Registro de Use Cases
            services.AddScoped<IEntrarUseCase, EntrarUseCase>();
            services.AddScoped<IAtualizarTokenUseCase, AtualizarTokenUseCase>();

            // Registro de Services
            services.AddScoped<IAutenticacaoService, AutenticacaoService>();

            // Registro de Validations
            services.AddValidatorsFromAssemblyContaining<EntrarValidation>();

            return services;
        }
    }
} 