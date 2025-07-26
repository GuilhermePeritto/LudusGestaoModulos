using System.Threading.Tasks;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<TokenResponseDTO> Entrar(EntrarDTO dto);
        Task<TokenResponseDTO> AtualizarToken(AtualizarTokenDTO dto);
    }
} 