using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using LudusGestao.Shared.Domain.Common;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.Interfaces;
using ludusGestao.Autenticacao.Application.Services;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.UseCases
{
    public class AtualizarTokenUseCase : BaseUseCase, IAtualizarTokenUseCase
    {
        private readonly JwtService _jwtService;

        public AtualizarTokenUseCase(JwtService jwtService, INotificador notificador)
            : base(notificador)
        {
            _jwtService = jwtService;
        }

        public async Task<TokenResponseDTO> Executar(AtualizarTokenDTO dto)
        {
            var principal = _jwtService.ValidarJwt(dto.RefreshToken);
            
            if (principal == null)
            {
                Notificar("Refresh token inválido ou expirado.");
                return null;
            }

            var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var emailClaim = principal.FindFirst(JwtRegisteredClaimNames.UniqueName)?.Value;
            var tenantIdClaim = principal.FindFirst("tenantId")?.Value;
            var tipoClaim = principal.FindFirst("tipo")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || string.IsNullOrEmpty(emailClaim) || 
                string.IsNullOrEmpty(tenantIdClaim) || tipoClaim != "refresh")
            {
                Notificar("Refresh token inválido.");
                return null;
            }

            if (!Guid.TryParse(userIdClaim, out var userId) || !int.TryParse(tenantIdClaim, out var tenantId))
            {
                Notificar("Refresh token inválido.");
                return null;
            }

            // Criar um usuário temporário para gerar o novo token
            var usuario = UsuarioAutenticacao.Criar(emailClaim, "");
            usuario.AlterarTenant(tenantId);

            var token = _jwtService.GerarJwt(usuario);
            var refreshToken = _jwtService.GerarRefreshToken(usuario);
            var expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos);

            return TokenResponseDTO.Criar(token, refreshToken, expiracao, usuario);
        }
    }
} 