using System;
using System.Threading.Tasks;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface IRemoverLocalUseCase
    {
        Task<bool> Executar(Guid id);
    }
} 