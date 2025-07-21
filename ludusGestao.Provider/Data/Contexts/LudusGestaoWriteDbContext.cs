using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Entities;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Entities;

namespace ludusGestao.Provider.Data.Contexts
{
    public class LudusGestaoWriteDbContext : DbContext
    {
        public LudusGestaoWriteDbContext(DbContextOptions<LudusGestaoWriteDbContext> options) : base(options)
        {
        }

        // DbSets para o módulo Gerais
        public DbSet<Local> Locais { get; set; }
        // DbSets para o módulo Autenticacao
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Filial> Filiais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações específicas para operações de escrita
            // - Desabilita lazy loading para melhor performance
            // - Configura tracking otimizado para comandos
            // - Aplica configurações de auditoria

            // Configurar auditoria automática
            ConfigureAuditProperties(modelBuilder);

            // Aplicar configurações específicas do módulo Eventos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Local).Assembly);
        }

        private void ConfigureAuditProperties(ModelBuilder modelBuilder)
        {
            // Configurar propriedades de auditoria para todas as entidades que herdam de EntityBase
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntidadeBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("DataCriacao")
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("DataAtualizacao")
                        .IsRequired(false);
                }
            }
        }

        // Auditoria automática removida temporariamente devido às propriedades serem protected set
        // A auditoria será feita através dos métodos Atualizar() das entidades
    }
} 