using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Application.Providers;

namespace ludusGestao.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;
        public TenantMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, ITenantContext tenantContext)
        {
            if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdStr) &&
                int.TryParse(tenantIdStr, out var tenantId))
            {
                tenantContext.SetTenantId(tenantId);
            }
            else
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("TenantId inválido ou ausente");
                return;
            }
            await _next(context);
        }
    }
} 