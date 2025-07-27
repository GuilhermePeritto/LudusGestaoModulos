using System;
using System.Threading.Tasks;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface IRemoverLocalUseCase
    {
        Task Executar(Guid id);
    }
} 