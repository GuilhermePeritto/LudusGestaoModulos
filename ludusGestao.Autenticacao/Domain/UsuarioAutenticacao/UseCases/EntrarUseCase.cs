using System;
using System.Threading.Tasks;
using BCrypt.Net;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Specifications;
using ludusGestao.Autenticacao.Application.Services;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.UseCases
{
    public class EntrarUseCase : BaseUseCase, IEntrarUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;

        public EntrarUseCase(
            IUsuarioAutenticacaoReadProvider usuarioProvider, 
            JwtService jwtService,
            INotificador notificador)
            : base(notificador)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
        }

        public async Task<TokenResponseDTO> Executar(EntrarDTO dto)
        {
            var usuario = await _usuarioProvider.ObterPorEmail(dto.Email);
            
            if (usuario == null)
            {
                Notificar("Usuário não encontrado.");
                return null;
            }

            if (!new UsuarioAtivoSpecification().IsSatisfiedBy(usuario))
            {
                Notificar("Usuário inativo.");
                return null;
            }

            if (!VerificarSenha(dto.Senha, usuario.Senha))
            {
                Notificar("Senha inválida.");
                return null;
            }

            var token = _jwtService.GerarJwt(usuario);
            var refreshToken = _jwtService.GerarRefreshToken(usuario);
            var expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos);

            return TokenResponseDTO.Criar(token, refreshToken, expiracao, usuario);
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
} 