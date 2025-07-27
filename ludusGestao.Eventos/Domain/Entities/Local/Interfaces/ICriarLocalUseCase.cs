using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Entities.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Entities.Local.Interfaces
{
    public interface ICriarLocalUseCase
    {
        Task<ludusGestao.Eventos.Domain.Entities.Local.Local> Executar(CriarLocalDTO dto);
    }
} 