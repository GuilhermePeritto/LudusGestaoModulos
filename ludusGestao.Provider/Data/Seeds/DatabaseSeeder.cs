using System;
using System.Linq;
using System.Threading.Tasks;
using ludusGestao.Provider.Data.Contexts;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Filial;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Eventos.Domain.Entities.Local;
using LudusGestao.Shared.Domain.ValueObjects;
using LudusGestao.Shared.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LudusGestao.Shared.Tenant;

namespace ludusGestao.Provider.Data.Seeds
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(LudusGestaoWriteDbContext context, IPasswordHelper passwordHelper = null)
        {
            // Empresa
            if (!context.Empresas.Any())
            {
                var empresa = Empresa.Criar(
                    nome: "Ludus",
                    email: "contato@ludussistemas.com.br",
                    telefone: "11999999999",
                    cnpj: "11.222.333/0001-81",
                    rua: "Rua Ludus",
                    numero: "100",
                    bairro: "Centro",
                    cidade: "São Paulo",
                    estado: "SP",
                    cep: "01000-000"
                );
                context.Empresas.Add(empresa);
                await context.SaveChangesAsync();
            }

            // Filial
            if (!context.Filiais.Any())
            {
                var empresa = context.Empresas.First();
                var filial = Filial.Criar(
                    nome: "Matriz",
                    rua: "Rua Ludus",
                    numero: "100",
                    bairro: "Centro",
                    cidade: "São Paulo",
                    estado: "SP",
                    cep: "01000-000",
                    telefone: "11999999999",
                    email: "matriz@ludussistemas.com.br",
                    cnpj: "11.222.333/0001-81",
                    responsavel: "Administrador Ludus",
                    dataAbertura: DateTime.UtcNow,
                    empresaId: empresa.Id
                );
                context.Filiais.Add(filial);
                await context.SaveChangesAsync();
            }

            // Usuário
            if (!context.Usuarios.Any())
            {
                var empresa = context.Empresas.First();
                var senhaHash = passwordHelper?.CriptografarSenha("Ludus@2024") ?? "Ludus@2024";
                var usuario = Usuario.Criar(
                    nome: "Administrador Ludus",
                    email: "admin@ludussistemas.com.br",
                    telefone: "11999999999",
                    cargo: "Administrador",
                    empresaId: empresa.Id,
                    senha: senhaHash,
                    rua: "Rua Ludus",
                    numero: "100",
                    bairro: "Centro",
                    cidade: "São Paulo",
                    estado: "SP",
                    cep: "01000-000"
                );
                context.Usuarios.Add(usuario);
                await context.SaveChangesAsync();
            }

            // Local
            if (!context.Locais.Any())
            {
                var local = Local.Criar(
                    nome: "Auditório Principal",
                    descricao: "Auditório principal da empresa com capacidade para 200 pessoas",
                    endereco: new Endereco("Rua Ludus", "100", "Centro", "São Paulo", "SP", "01000-000"),
                    telefone: new Telefone("11999999999")
                );
                context.Locais.Add(local);
                await context.SaveChangesAsync();
            }
        }
    }
} 