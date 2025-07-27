using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IAtualizarTokenUseCase
    {
        Task<TokenResponseDTO> Executar(AtualizarTokenDTO dto);
    }
} 