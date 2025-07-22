using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Entities;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Application.Providers;

namespace ludusGestao.Provider.Data.Contexts
{
    public class LudusGestaoReadDbContext : DbContext
    {
        private readonly ITenantContext _tenantContext;
        public LudusGestaoReadDbContext(DbContextOptions<LudusGestaoReadDbContext> options, ITenantContext tenantContext) : base(options)
        {
            _tenantContext = tenantContext;
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

            // Filtro global multitenant
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IEntidadeTenant).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(LudusGestaoReadDbContext).GetMethod(nameof(SetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .MakeGenericMethod(entityType.ClrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }

            // Configurações específicas para operações de leitura
            // - Desabilita change tracking para melhor performance
            // - Configura queries otimizadas
            // - Aplica configurações de auditoria somente leitura

            // Configurar auditoria somente leitura
            ConfigureReadOnlyAuditProperties(modelBuilder);

            // Aplicar configurações específicas do módulo Eventos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Local).Assembly);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            // Configurações específicas para leitura
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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
                        .Property("DataAtualizacao")
                        .IsRequired(false);
                }
            }
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntidadeTenant
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
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