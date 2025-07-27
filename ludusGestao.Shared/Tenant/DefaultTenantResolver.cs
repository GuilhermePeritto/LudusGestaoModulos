using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace LudusGestao.Shared.Tenant
{
    public class TenantResolverOptions
    {
        public List<string> PublicRoutes { get; set; } = new()
        {
            "/api/autenticacao",
            "/api/auth",
            "/api/health",
            "/swagger",
            "/favicon.ico"
        };
        
        public int CacheExpirationMinutes { get; set; } = 30;
        public bool EnableTenantValidation { get; set; } = true;
    }

    public class DefaultTenantResolver : ITenantResolver
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<DefaultTenantResolver> _logger;
        private readonly TenantResolverOptions _options;

        public DefaultTenantResolver(
            IMemoryCache cache,
            ILogger<DefaultTenantResolver> logger,
            IOptions<TenantResolverOptions> options)
        {
            _cache = cache;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<TenantInfo> ResolveTenantAsync(string tenantId)
        {
            var cacheKey = $"tenant_info_{tenantId}";
            
            if (_cache.TryGetValue(cacheKey, out TenantInfo cachedInfo))
            {
                return cachedInfo;
            }

            // Simular busca do tenant (em produção, isso viria do banco de dados)
            var tenantInfo = await LoadTenantInfoAsync(tenantId);
            
            if (tenantInfo != null)
            {
                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
                };
                
                _cache.Set(cacheKey, tenantInfo, cacheOptions);
            }

            return tenantInfo;
        }

        public async Task<bool> IsValidTenantAsync(int tenantId)
        {
            if (!_options.EnableTenantValidation)
                return true;

            var cacheKey = $"tenant_valid_{tenantId}";
            
            if (_cache.TryGetValue(cacheKey, out bool cachedValid))
            {
                return cachedValid;
            }

            // Simular validação do tenant (em produção, isso viria do banco de dados)
            var isValid = await ValidateTenantAsync(tenantId);
            
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_options.CacheExpirationMinutes)
            };
            
            _cache.Set(cacheKey, isValid, cacheOptions);
            
            return isValid;
        }

        public async Task<bool> IsPublicRouteAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;

            return await Task.FromResult(_options.PublicRoutes.Any(route => 
                path.StartsWith(route, StringComparison.OrdinalIgnoreCase)));
        }

        private async Task<TenantInfo> LoadTenantInfoAsync(string tenantId)
        {
            // Simular carregamento do tenant (em produção, isso viria do banco de dados)
            await Task.Delay(10); // Simular latência de rede
            
            if (!int.TryParse(tenantId, out var id))
                return null;

            // Simular dados do tenant
            return new TenantInfo
            {
                Id = id,
                Name = $"Tenant {id}",
                IsActive = true,
                ConnectionString = "default_connection_string",
                Settings = new Dictionary<string, object>
                {
                    ["theme"] = "default",
                    ["timezone"] = "UTC-3",
                    ["language"] = "pt-BR"
                }
            };
        }

        private async Task<bool> ValidateTenantAsync(int tenantId)
        {
            // Simular validação do tenant (em produção, isso viria do banco de dados)
            await Task.Delay(5); // Simular latência de rede
            
            // Simular que tenants com ID > 0 são válidos
            return tenantId > 0;
        }

        public void ClearCache()
        {
            if (_cache is MemoryCache memoryCache)
            {
                memoryCache.Compact(1.0);
            }
        }
    }
} 