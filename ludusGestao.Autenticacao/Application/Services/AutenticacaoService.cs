using System.Threading.Tasks;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.UseCases;
using ludusGestao.Autenticacao.Application.Validations;

namespace ludusGestao.Autenticacao.Application.Services
{
    public class AutenticacaoService
    {
        private readonly EntrarUseCase _entrarUseCase;
        private readonly AtualizarTokenUseCase _atualizarTokenUseCase;
        private readonly EntrarValidation _entrarValidation;
        private readonly AtualizarTokenValidation _atualizarTokenValidation;

        public AutenticacaoService(
            EntrarUseCase entrarUseCase,
            AtualizarTokenUseCase atualizarTokenUseCase,
            EntrarValidation entrarValidation,
            AtualizarTokenValidation atualizarTokenValidation)
        {
            _entrarUseCase = entrarUseCase;
            _atualizarTokenUseCase = atualizarTokenUseCase;
            _entrarValidation = entrarValidation;
            _atualizarTokenValidation = atualizarTokenValidation;
        }

        public async Task<(bool valido, TokenResponseDTO? resposta, string? erro)> Entrar(EntrarDTO dto)
        {
            var validation = _entrarValidation.Validate(dto);
            if (!validation.IsValid)
                return (false, null, validation.Errors[0].ErrorMessage);
            var result = await _entrarUseCase.Executar(dto);
            if (result == null)
                return (false, null, "Usuário ou senha inválidos.");
            return (true, result, null);
        }

        public async Task<(bool valido, TokenResponseDTO? resposta, string? erro)> AtualizarToken(AtualizarTokenDTO dto)
        {
            var validation = _atualizarTokenValidation.Validate(dto);
            if (!validation.IsValid)
                return (false, null, validation.Errors[0].ErrorMessage);
            var result = await _atualizarTokenUseCase.Executar(dto);
            if (result == null)
                return (false, null, "Refresh token inválido ou expirado.");
            return (true, result, null);
        }
    }
} 