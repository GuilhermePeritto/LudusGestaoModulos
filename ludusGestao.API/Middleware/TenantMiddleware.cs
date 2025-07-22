using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Application.Providers;
using LudusGestao.Shared.Application.Responses;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace ludusGestao.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ITenantContext tenantContext)
        {
            // Ignorar validação de tenant para rotas públicas (ex: autenticação)
            var path = context.Request.Path.Value?.ToLower();
            if (path != null && (path.StartsWith("/api/autenticacao") || path.StartsWith("/api/auth")))
            {
                var tenantContextService = context.RequestServices.GetService(typeof(LudusGestao.Shared.Application.Providers.ITenantContext)) as LudusGestao.Shared.Application.Providers.ITenantContext;
                if (tenantContextService != null)
                    tenantContextService.IgnorarFiltro(true); // Ignora filtro de tenant para rotas públicas
                await _next(context);
                return;
            }
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdStr) &&
                int.TryParse(tenantIdStr, out var tenantId))
            {
                tenantContext.SetTenantId(tenantId);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                var resposta = new RespostaBase<object>(null)
                {
                    Sucesso = false,
                    Mensagem = "TenantId inválido ou ausente",
                    Erros = new System.Collections.Generic.List<string> { "TenantId inválido ou ausente" }
                };
                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(resposta));
                return;
            }
            await _next(context);
        }
    }
} 