using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using ludusGestao.Gerais.Domain.Empresa.Interfaces;
using ludusGestao.Gerais.Domain.Empresa.UseCases;
using ludusGestao.Gerais.Domain.Empresa.Validations;
using ludusGestao.Gerais.Application.Services;
using ludusGestao.Gerais.Domain.Usuario.Interfaces;
using ludusGestao.Gerais.Domain.Usuario.UseCases;
using ludusGestao.Gerais.Domain.Usuario.Validations;
using ludusGestao.Gerais.Domain.Filial.Interfaces;
using ludusGestao.Gerais.Domain.Filial.UseCases;
using ludusGestao.Gerais.Domain.Filial.Validations;

namespace ludusGestao.Gerais.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGeraisModule(this IServiceCollection services)
        {
            // Registro de Use Cases - Empresa
            services.AddScoped<ICriarEmpresaUseCase, CriarEmpresaUseCase>();
            services.AddScoped<IAtualizarEmpresaUseCase, AtualizarEmpresaUseCase>();
            services.AddScoped<IRemoverEmpresaUseCase, RemoverEmpresaUseCase>();
            services.AddScoped<IBuscarEmpresaPorIdUseCase, BuscarEmpresaPorIdUseCase>();
            services.AddScoped<IListarEmpresasUseCase, ListarEmpresasUseCase>();

            // Registro de Use Cases - Filial
            services.AddScoped<ICriarFilialUseCase, CriarFilialUseCase>();
            services.AddScoped<IAtualizarFilialUseCase, AtualizarFilialUseCase>();
            services.AddScoped<IRemoverFilialUseCase, RemoverFilialUseCase>();
            services.AddScoped<IBuscarFilialPorIdUseCase, BuscarFilialPorIdUseCase>();
            services.AddScoped<IListarFiliaisUseCase, ListarFiliaisUseCase>();

            // Registro de Use Cases - Usuario
            services.AddScoped<ICriarUsuarioUseCase, CriarUsuarioUseCase>();
            services.AddScoped<IAtualizarUsuarioUseCase, AtualizarUsuarioUseCase>();
            services.AddScoped<IRemoverUsuarioUseCase, RemoverUsuarioUseCase>();
            services.AddScoped<IBuscarUsuarioPorIdUseCase, BuscarUsuarioPorIdUseCase>();
            services.AddScoped<IListarUsuariosUseCase, ListarUsuariosUseCase>();

            // Registro de Services
            services.AddScoped<IEmpresaService, EmpresaService>();
            services.AddScoped<IFilialService, FilialService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            // Registro de Validations
            services.AddValidatorsFromAssemblyContaining<CriarEmpresaValidation>();

            return services;
        }
    }
} 