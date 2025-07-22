using LudusGestao.Shared.Application.Events;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace ludusGestao.API.Extensions
{
    public static class EventBusExtensions
    {
        public static void RegistrarHandlersGlobais(this IEventBus eventBus, IHttpContextAccessor accessor)
        {
            // Handler de log
            eventBus.RegistrarHandler<ErroEvento>(async erro =>
            {
                // Exemplo: logar no console (pode ser substituído por log estruturado)
                Debug.WriteLine($"[ERRO] {erro.Codigo} - {erro.Mensagem} - {erro.StatusCode}");
                await Task.CompletedTask;
            });

            // Handler de resposta padronizada
            eventBus.RegistrarHandler<ErroEvento>(async erro =>
            {
                var context = accessor.HttpContext;
                if (context != null)
                {
                    context.Items["ErroEvento"] = erro;
                }
                await Task.CompletedTask;
            });
        }
    }
} 