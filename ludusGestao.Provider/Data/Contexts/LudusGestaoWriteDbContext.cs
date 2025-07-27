using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Entities.Local;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Usuario;
using ludusGestao.Gerais.Domain.Empresa;
using ludusGestao.Gerais.Domain.Filial;
using LudusGestao.Shared.Tenant;

namespace ludusGestao.Provider.Data.Contexts
{
    public class LudusGestaoWriteDbContext : DbContext
    {
        private readonly ITenantContext _tenantContext;
        public LudusGestaoWriteDbContext(DbContextOptions<LudusGestaoWriteDbContext> options, ITenantContext tenantContext) : base(options)
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

            // Aplicar filtro multitenant otimizado
            ApplyTenantFilters(modelBuilder);

            // Configurar auditoria automática
            ConfigureAuditProperties(modelBuilder);

            // Aplicar configurações específicas do módulo Eventos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Local).Assembly);
            // Aplicar configurações específicas do módulo Gerais
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ludusGestao.Provider.Data.Configurations.Gerais.EmpresaConfiguration).Assembly);
        }

        private void ApplyTenantFilters(ModelBuilder modelBuilder)
        {
            // Aplicar filtro para cada entidade que herda de EntidadeBase
            ApplyTenantFilter<Local>(modelBuilder);
            ApplyTenantFilter<Usuario>(modelBuilder);
            ApplyTenantFilter<Empresa>(modelBuilder);
            ApplyTenantFilter<Filial>(modelBuilder);
        }

        private void ApplyTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : EntidadeBase
        {
            var ignorarFiltro = _tenantContext.IgnorarFiltroTenant;
            var tenantId = _tenantContext.TenantIdNullable;
            
            TenantFilterBuilder.ApplyTenantFilter(modelBuilder.Entity<TEntity>(), tenantId, ignorarFiltro);
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
                        .Property("DataAlteracao")
                        .IsRequired(false);
                }
            }
        }

        public override int SaveChanges()
        {
            // Aplicar TenantId automaticamente para novas entidades
            foreach (var entry in ChangeTracker.Entries<EntidadeBase>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.AlterarTenant(_tenantContext.TenantId);
                entry.Entity.MarcarAlterado();
            }

            // Marcar como alterado para entidades modificadas
            foreach (var entry in ChangeTracker.Entries<EntidadeBase>().Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.MarcarAlterado();
            }

            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Aplicar TenantId automaticamente para novas entidades
            foreach (var entry in ChangeTracker.Entries<EntidadeBase>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.AlterarTenant(_tenantContext.TenantId);
                entry.Entity.MarcarAlterado();
            }

            // Marcar como alterado para entidades modificadas
            foreach (var entry in ChangeTracker.Entries<EntidadeBase>().Where(e => e.State == EntityState.Modified))
            {
                entry.Entity.MarcarAlterado();
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
} 