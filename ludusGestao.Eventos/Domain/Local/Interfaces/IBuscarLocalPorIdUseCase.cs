using System;
using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface IBuscarLocalPorIdUseCase
    {
        Task<Local> Executar(Guid id);
    }
} 