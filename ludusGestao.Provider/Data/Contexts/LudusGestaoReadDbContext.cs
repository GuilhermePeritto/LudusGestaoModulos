using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Local;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Filial;
using LudusGestao.Shared.Tenant;
using Microsoft.Extensions.Logging;

namespace ludusGestao.Provider.Data.Contexts
{
    public class LudusGestaoReadDbContext : DbContext
    {
        private readonly ITenantContext _tenantContext;
        public LudusGestaoReadDbContext(DbContextOptions<LudusGestaoReadDbContext> options, ITenantContext tenantContext) : base(options)
        {
            _tenantContext = tenantContext;
        }

        // DbSets para o módulo Eventos
        public DbSet<Local> Locais { get; set; }
        // DbSets para o módulo Gerais
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Filial> Filiais { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar auditoria somente leitura
            ConfigureReadOnlyAuditProperties(modelBuilder);

            // Aplicar configurações específicas do módulo Eventos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Local).Assembly);
            // Aplicar configurações específicas do módulo Gerais
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ludusGestao.Provider.Data.Configurations.Gerais.EmpresaConfiguration).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Configurações específicas para leitura
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

            // Log de SQL para debug
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
        }

        private void ConfigureReadOnlyAuditProperties(ModelBuilder modelBuilder)
        {
            // Configurar propriedades de auditoria para todas as entidades que herdam de EntityBase
            // Em modo somente leitura, essas propriedades são apenas para consulta
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntidadeBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property("DataCriacao")
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property("DataAlteracao")
                        .IsRequired(false);
                }
            }
        }

        // Sobrescrever SaveChanges para prevenir modificações acidentais
        public override int SaveChanges()
        {
            throw new InvalidOperationException("Este contexto é somente para leitura. Use LudusGestaoWriteDbContext para operações de escrita.");
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException("Este contexto é somente para leitura. Use LudusGestaoWriteDbContext para operações de escrita.");
        }
    }
} 