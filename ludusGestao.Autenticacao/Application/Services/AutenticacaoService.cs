using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;

namespace ludusGestao.Autenticacao.Application.Services
{
    public class AutenticacaoService : BaseService, IAutenticacaoService
    {
        private readonly IEntrarUseCase _entrarUseCase;
        private readonly IAtualizarTokenUseCase _atualizarTokenUseCase;

        public AutenticacaoService(
            IEntrarUseCase entrarUseCase,
            IAtualizarTokenUseCase atualizarTokenUseCase,
            INotificador notificador)
            : base(notificador)
        {
            _entrarUseCase = entrarUseCase;
            _atualizarTokenUseCase = atualizarTokenUseCase;
        }

        public async Task<TokenResponseDTO> Entrar(EntrarDTO dto)
        {
            var result = await _entrarUseCase.Executar(dto);
            
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public async Task<TokenResponseDTO> AtualizarToken(AtualizarTokenDTO dto)
        {
            var result = await _atualizarTokenUseCase.Executar(dto);
            
            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
} 