using System;
using System.Threading.Tasks;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface IBuscarLocalPorIdUseCase
    {
        Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Executar(Guid id);
    }
} 