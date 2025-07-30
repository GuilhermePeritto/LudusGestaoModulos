using System;
using System.Threading.Tasks;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface IBuscarLocalPorIdUseCase
    {
        Task<ludusGestao.Eventos.Domain.Local.Local> Executar(Guid id);
    }
} 