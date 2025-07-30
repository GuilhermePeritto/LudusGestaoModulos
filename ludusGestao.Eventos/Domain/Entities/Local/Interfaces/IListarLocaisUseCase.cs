using System.Collections.Generic;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface IListarLocaisUseCase
    {
        Task<IEnumerable<LocalDTO>> Executar();
    }
} 