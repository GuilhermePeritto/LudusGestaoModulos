using Microsoft.EntityFrameworkCore;
using ludusGestao.Eventos.Domain.Entities;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Entities;
using LudusGestao.Shared.Application.Providers;

namespace ludusGestao.Provider.Data.Contexts
{
    public class LudusGestaoWriteDbContext : DbContext
    {
        private readonly ITenantContext _tenantContext;
        public LudusGestaoWriteDbContext(DbContextOptions<LudusGestaoWriteDbContext> options, ITenantContext tenantContext) : base(options)
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
                    var method = typeof(LudusGestaoWriteDbContext).GetMethod(nameof(SetTenantFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                        .MakeGenericMethod(entityType.ClrType);
                    method.Invoke(this, new object[] { modelBuilder });
                }
            }

            // Configurações específicas para operações de escrita
            // - Desabilita lazy loading para melhor performance
            // - Configura tracking otimizado para comandos
            // - Aplica configurações de auditoria

            // Configurar auditoria automática
            ConfigureAuditProperties(modelBuilder);

            // Aplicar configurações específicas do módulo Eventos
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Local).Assembly);
            // Aplicar configurações específicas do módulo Gerais
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ludusGestao.Provider.Data.Configurations.Gerais.EmpresaConfiguration).Assembly);
        }

        private void SetTenantFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntidadeTenant
        {
            var tenantContext = _tenantContext as LudusGestao.Shared.Application.Providers.TenantContext;
            var ignorarFiltro = tenantContext != null && tenantContext.IgnorarFiltroTenant;
            modelBuilder.Entity<TEntity>().HasQueryFilter(e =>
                !ignorarFiltro ? e.TenantId == _tenantContext.TenantId : true
            );
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<IEntidadeTenant>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = _tenantContext.TenantId;
            }
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<IEntidadeTenant>().Where(e => e.State == EntityState.Added))
            {
                entry.Entity.TenantId = _tenantContext.TenantId;
            }
            return await base.SaveChangesAsync(cancellationToken);
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
                        .Property("DataAlteracao") // Corrigido aqui
                        .IsRequired(false);
                }
            }
        }

        // Auditoria automática removida temporariamente devido às propriedades serem protected set
        // A auditoria será feita através dos métodos Atualizar() das entidades
    }
} 