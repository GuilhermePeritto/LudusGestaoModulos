using System;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using LudusGestao.Shared.Notificacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Validations;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Specifications;
using ludusGestao.Autenticacao.Application.Services;
using LudusGestao.Shared.Security;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.UseCases
{
    public class EntrarUseCase : BaseUseCase, IEntrarUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;
        private readonly IPasswordHelper _passwordHelper;

        public EntrarUseCase(
            IUsuarioAutenticacaoReadProvider usuarioProvider, 
            JwtService jwtService,
            IPasswordHelper passwordHelper,
            INotificador notificador)
            : base(notificador)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
            _passwordHelper = passwordHelper;
        }

        public async Task<TokenResponseDTO> Executar(EntrarDTO dto)
        {
            // Validação básica de formato
            if (!ExecutarValidacao(new EntrarValidation(), dto))
                return null;

            // Buscar usuário
            var usuario = await _usuarioProvider.ObterPorEmail(dto.Email);
            if (usuario == null)
            {
                Notificar("Usuário não encontrado.");
                return null;
            }

            // Verificar se usuário está ativo
            if (!new UsuarioAtivoSpecification().IsSatisfiedBy(usuario))
            {
                Notificar("Usuário inativo.");
                return null;
            }

            // Verificar senha
            if (!_passwordHelper.VerificarSenha(dto.Senha, usuario.Senha))
            {
                Notificar("Senha inválida.");
                return null;
            }
            
            var token = _jwtService.GerarJwt(usuario);
            var refreshToken = _jwtService.GerarRefreshToken(usuario);
            var expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos);

            return TokenResponseDTO.Criar(token, refreshToken, expiracao, usuario);
        }
    }
} 
