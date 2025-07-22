using System.Threading.Tasks;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.UseCases;
using ludusGestao.Autenticacao.Application.Validations;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Application.Events;
using System.Collections.Generic;

namespace ludusGestao.Autenticacao.Application.Services
{
    public class AutenticacaoService
    {
        private readonly EntrarUseCase _entrarUseCase;
        private readonly AtualizarTokenUseCase _atualizarTokenUseCase;
        private readonly EntrarValidation _entrarValidation;
        private readonly AtualizarTokenValidation _atualizarTokenValidation;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AutenticacaoService(
            EntrarUseCase entrarUseCase,
            AtualizarTokenUseCase atualizarTokenUseCase,
            EntrarValidation entrarValidation,
            AtualizarTokenValidation atualizarTokenValidation,
            IHttpContextAccessor httpContextAccessor)
        {
            _entrarUseCase = entrarUseCase;
            _atualizarTokenUseCase = atualizarTokenUseCase;
            _entrarValidation = entrarValidation;
            _atualizarTokenValidation = atualizarTokenValidation;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponseDTO?> Entrar(EntrarDTO dto)
        {
            var validation = _entrarValidation.Validate(dto);
            if (!validation.IsValid)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = "Erros de validação.",
                    StatusCode = 400,
                    Erros = validation.Errors.ConvertAll(e => e.ErrorMessage)
                });
                return null;
            }
            var result = await _entrarUseCase.Executar(dto);
            if (result == null)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "AUTENTICACAO_INVALIDA",
                    Mensagem = "Usuário ou senha inválidos.",
                    StatusCode = 400,
                    Erros = new List<string> { "Usuário ou senha inválidos." }
                });
                return null;
            }
            return result;
        }

        public async Task<TokenResponseDTO?> AtualizarToken(AtualizarTokenDTO dto)
        {
            var validation = _atualizarTokenValidation.Validate(dto);
            if (!validation.IsValid)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "VALIDACAO",
                    Mensagem = "Erros de validação.",
                    StatusCode = 400,
                    Erros = validation.Errors.ConvertAll(e => e.ErrorMessage)
                });
                return null;
            }
            var result = await _atualizarTokenUseCase.Executar(dto);
            if (result == null)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "REFRESH_TOKEN_INVALIDO",
                    Mensagem = "Refresh token inválido ou expirado.",
                    StatusCode = 400,
                    Erros = new List<string> { "Refresh token inválido ou expirado." }
                });
                return null;
            }
            return result;
        }
    }
} 