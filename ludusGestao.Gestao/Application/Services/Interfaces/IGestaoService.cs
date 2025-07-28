using System.Threading.Tasks;
using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;

namespace ludusGestao.Gestao.Application.Services.Interfaces
{
    public interface IGestaoService
    {
        Task<ClienteCriadoDTO> CriarCliente(CriarClienteDTO dto);
    }
} 