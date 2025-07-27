using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using LudusGestao.Shared.Tenant;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Migrations
{
    public class LudusGestaoDbContextFactory : IDesignTimeDbContextFactory<LudusGestaoWriteDbContext>
    {
        public LudusGestaoWriteDbContext CreateDbContext(string[] args)
        {
            // Buscar o appsettings.json do projeto API
            var currentDir = Directory.GetCurrentDirectory();
            
            // Tentar diferentes caminhos possíveis
            var possiblePaths = new[]
            {
                Path.Combine(currentDir, "..", "..", "..", "ludusGestao.API"),
                Path.Combine(currentDir, "..", "..", "ludusGestao.API"),
                Path.Combine(currentDir, "..", "ludusGestao.API"),
                Path.Combine(currentDir, "ludusGestao.API")
            };
            
            string apiProjectPath = null;
            foreach (var path in possiblePaths)
            {
                var appsettingsPath = Path.Combine(path, "appsettings.json");
                if (File.Exists(appsettingsPath))
                {
                    apiProjectPath = path;
                    break;
                }
            }
            
            if (apiProjectPath == null)
            {
                throw new FileNotFoundException($"Arquivo appsettings.json não encontrado. Diretório atual: {currentDir}");
            }
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<LudusGestaoWriteDbContext>();
            
            // Pegar connection string do appsettings
            var connectionString = configuration.GetSection("Database:Write:ConnectionString").Value;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'Database:Write:ConnectionString' não encontrada no appsettings.json");
            }
            
            optionsBuilder.UseNpgsql(connectionString);

            // Criar um tenant context simples para design-time
            var tenantContext = new DesignTimeTenantContext();

            return new LudusGestaoWriteDbContext(optionsBuilder.Options, tenantContext);
        }
    }

    // TenantContext simples para design-time
    public class DesignTimeTenantContext : ITenantContext
    {
        public int TenantId => 1;
        public int? TenantIdNullable => 1;
        public TenantInfo TenantInfo => new TenantInfo { Id = 1, Name = "Design-Time", IsActive = true };
        public bool IgnorarFiltroTenant => true;
        
        public void SetTenantId(int tenantId) { }
        public void SetTenantInfo(TenantInfo tenantInfo) { }
        public void IgnorarFiltro(bool ignorar) { }
        public void Clear() { }
    }
} 