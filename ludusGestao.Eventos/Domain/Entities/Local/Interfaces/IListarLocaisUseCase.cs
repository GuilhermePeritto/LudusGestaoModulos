using System.Collections.Generic;
using System.Threading.Tasks;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface IListarLocaisUseCase
    {
        Task<IEnumerable<ludusGestao.Eventos.Domain.Entities.Local.Local>> Executar();
    }
} 