using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface IAtualizarLocalUseCase
    {
        Task<Local> Executar(Local local);
    }
} 