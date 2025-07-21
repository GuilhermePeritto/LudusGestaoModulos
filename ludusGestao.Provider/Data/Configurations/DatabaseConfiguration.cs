using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ludusGestao.Provider.Data.Contexts;

namespace ludusGestao.Provider.Data.Configurations
{
    public static class DatabaseConfiguration
    {
        public static IServiceCollection AddDatabaseConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Configuração do banco de dados de escrita
            services.AddDbContext<LudusGestaoWriteDbContext>(options =>
            {
                ConfigureWriteDbContext(options, configuration);
            });

            // Configuração do banco de dados de leitura
            services.AddDbContext<LudusGestaoReadDbContext>(options =>
            {
                ConfigureReadDbContext(options, configuration);
            });

            return services;
        }

        private static void ConfigureWriteDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
        {
            var writeSection = configuration.GetSection("Database:Write");
            var provider = writeSection["Provider"]?.ToLower();
            var connectionString = writeSection["ConnectionString"];

            switch (provider)
            {
                case "sqlserver":
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                    break;
                case "postgres":
                    options.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        // Configurações específicas do Npgsql
                    });
                    break;
                // Adicione outros providers conforme necessário
                default:
                    throw new Exception($"Provider de escrita '{provider}' não suportado");
            }

            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
        }

        private class ReadDbConfig
        {
            public string Provider { get; set; }
            public string ConnectionString { get; set; }
        }

        private static void ConfigureReadDbContext(DbContextOptionsBuilder options, IConfiguration configuration)
        {
            var readsSection = configuration.GetSection("Database:Reads");
            var reads = readsSection.Get<ReadDbConfig[]>();
            if (reads == null || reads.Length == 0)
                throw new Exception("Nenhuma configuração de banco de leitura encontrada");

            // Sorteia um dos bancos de leitura
            var rnd = new Random();
            var readConfig = reads[rnd.Next(reads.Length)];
            var provider = readConfig.Provider?.ToLower();
            var connectionString = readConfig.ConnectionString;

            switch (provider)
            {
                case "sqlserver":
                    options.UseSqlServer(connectionString, sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 3,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null);
                    });
                    break;
                case "postgres":
                    options.UseNpgsql(connectionString, npgsqlOptions =>
                    {
                        // Configurações específicas do Npgsql
                    });
                    break;
                // Adicione outros providers conforme necessário
                default:
                    throw new Exception($"Provider de leitura '{provider}' não suportado");
            }

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
        }
    }
} 