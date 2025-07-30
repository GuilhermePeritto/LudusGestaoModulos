using System.Threading.Tasks;
using ludusGestao.Eventos.Domain.Local.DTOs;

namespace ludusGestao.Eventos.Domain.Local.Interfaces
{
    public interface ICriarLocalUseCase
    {
        Task<ludusGestao.Eventos.Domain.Local.Local> Executar(CriarLocalDTO dto);
    }
} 