using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IEntrarUseCase
    {
        Task<TokenResponseDTO> Executar(EntrarDTO dto);
    }
} 