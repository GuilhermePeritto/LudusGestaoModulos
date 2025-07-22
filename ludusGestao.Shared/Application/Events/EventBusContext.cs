using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using LudusGestao.Shared.Application.Events;

namespace LudusGestao.Shared.Application.Events
{
    public static class EventBusContext
    {
        public static async Task PublicarErro(this IHttpContextAccessor accessor, ErroEvento erro)
        {
            var eventBus = accessor.HttpContext?.RequestServices.GetService(typeof(IEventBus)) as IEventBus;
            if (eventBus != null)
            {
                await eventBus.Publicar(erro);
            }
        }
    }
} 