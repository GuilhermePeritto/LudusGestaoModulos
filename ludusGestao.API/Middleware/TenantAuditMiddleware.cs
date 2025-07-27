using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using LudusGestao.Shared.Tenant;
using System.Diagnostics;

namespace ludusGestao.API.Middleware
{
    public class TenantAuditMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TenantAuditMiddleware> _logger;

        public TenantAuditMiddleware(RequestDelegate next, ILogger<TenantAuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var originalTenantId = GetTenantIdFromRequest(context);
            
            try
            {
                await _next(context);
            }
            finally
            {
                stopwatch.Stop();
                
                var currentTenantId = GetTenantIdFromContext(context);
                var path = context.Request.Path.Value;
                var method = context.Request.Method;
                var statusCode = context.Response.StatusCode;
                var duration = stopwatch.ElapsedMilliseconds;

                // Log de auditoria
                LogTenantAccess(originalTenantId, currentTenantId, path, method, statusCode, duration, context);
            }
        }

        private int? GetTenantIdFromRequest(HttpContext context)
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

        private int? GetTenantIdFromContext(HttpContext context)
        {
            var tenantContext = context.RequestServices.GetService<ITenantContext>();
            return tenantContext?.TenantId;
        }

        private void LogTenantAccess(int? originalTenantId, int? currentTenantId, string path, string method, int statusCode, long duration, HttpContext context)
        {
            var logLevel = statusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            
            var message = "Tenant Access: {Method} {Path} | Original: {OriginalTenantId} | Current: {CurrentTenantId} | Status: {StatusCode} | Duration: {Duration}ms";
            
            _logger.Log(logLevel, message, 
                method, 
                path, 
                originalTenantId?.ToString() ?? "none", 
                currentTenantId?.ToString() ?? "none", 
                statusCode, 
                duration);

            // Log de segurança para tentativas suspeitas
            if (originalTenantId != currentTenantId && originalTenantId.HasValue && currentTenantId.HasValue)
            {
                _logger.LogWarning("POSSIBLE TENANT SWITCHING ATTEMPT: Original={OriginalTenantId}, Current={CurrentTenantId}, Path={Path}, User={User}", 
                    originalTenantId, currentTenantId, path, context.User?.Identity?.Name ?? "anonymous");
            }

            // Log de performance para operações lentas
            if (duration > 1000) // Mais de 1 segundo
            {
                _logger.LogWarning("SLOW TENANT OPERATION: {Method} {Path} | Tenant: {TenantId} | Duration: {Duration}ms", 
                    method, path, currentTenantId?.ToString() ?? "none", duration);
            }
        }
    }
} 