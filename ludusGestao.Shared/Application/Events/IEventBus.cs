using System.Threading.Tasks;

namespace LudusGestao.Shared.Application.Events
{
    public interface IEventBus
    {
        Task Publicar<T>(T evento) where T : class;
        void RegistrarHandler<T>(Func<T, Task> handler) where T : class;
    }
} 