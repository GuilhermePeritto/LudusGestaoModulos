using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Tenant;
using LudusGestao.Shared.Domain.Responses;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace ludusGestao.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantMiddleware> _logger;
        private readonly ITenantResolver _tenantResolver;

        public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger, ITenantResolver tenantResolver)
        {
            _next = next;
            _logger = logger;
            _tenantResolver = tenantResolver;
        }

        public async Task Invoke(HttpContext context, ITenantContext tenantContext)
        {
            var path = context.Request.Path.Value?.ToLower();
            
            // Verificar se é rota pública
            if (path != null && await _tenantResolver.IsPublicRouteAsync(path))
            {
                tenantContext.IgnorarFiltro(true);
                await _next(context);
                return;
            }

            // Extrair tenantId do header ou JWT
            var tenantId = await ExtractTenantIdAsync(context);
            
            if (tenantId == null)
            {
                await ReturnTenantErrorAsync(context, "TenantId inválido ou ausente");
                return;
            }

            // Validar se o tenant existe e está ativo
            if (!await _tenantResolver.IsValidTenantAsync(tenantId.Value))
            {
                _logger.LogWarning("Tentativa de acesso com tenant inválido: {TenantId}", tenantId);
                await ReturnTenantErrorAsync(context, "Tenant inválido ou inativo");
                return;
            }

            // Resolver informações completas do tenant
            var tenantInfo = await _tenantResolver.ResolveTenantAsync(tenantId.ToString());
            if (tenantInfo == null)
            {
                await ReturnTenantErrorAsync(context, "Não foi possível resolver informações do tenant");
                return;
            }

            // Configurar contexto do tenant
            tenantContext.SetTenantId(tenantId.Value);
            tenantContext.SetTenantInfo(tenantInfo);

            _logger.LogDebug("Tenant configurado: {TenantId} - {TenantName}", tenantId, tenantInfo.Name);

            await _next(context);
        }

        private async Task<int?> ExtractTenantIdAsync(HttpContext context)
        {
            // Extrair apenas do JWT
            var user = context.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                var tenantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "tenantId")?.Value;
                if (!string.IsNullOrEmpty(tenantIdClaim) && int.TryParse(tenantIdClaim, out var tenantId))
                {
                    return tenantId;
                }
            }

            return null;
        }

        private async Task ReturnTenantErrorAsync(HttpContext context, string message)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            
            var resposta = new RespostaBase(null, message, new System.Collections.Generic.List<string> { message })
            {
                Sucesso = false
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(resposta));
        }
    }
} 