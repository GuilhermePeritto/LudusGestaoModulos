using ludusGestao.Gestao.Domain.Entities.Cliente.DTOs;

namespace ludusGestao.Gestao.Domain.Entities.Cliente.Interfaces
{
    public interface ICriarClienteUseCase
    {
        Task<ClienteCriadoDTO> Executar(CriarClienteDTO dto);
    }
} 