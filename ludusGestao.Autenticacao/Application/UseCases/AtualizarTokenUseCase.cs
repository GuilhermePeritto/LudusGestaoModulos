using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.Services;
using ludusGestao.Autenticacao.Domain.Providers;
using ludusGestao.Autenticacao.Domain.Entities;

namespace ludusGestao.Autenticacao.Application.UseCases
{
    public class AtualizarTokenUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;

        public AtualizarTokenUseCase(IUsuarioAutenticacaoReadProvider usuarioProvider, JwtService jwtService)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
        }

        public async Task<TokenResponseDTO?> Executar(AtualizarTokenDTO dto)
        {
            var principal = _jwtService.ValidarJwt(dto.RefreshToken);
            if (principal == null)
                return null;

            var tipoClaim = principal.Claims.FirstOrDefault(c => c.Type == "tipo")?.Value;
            if (tipoClaim != "refresh")
                return null;

            var login = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            var tenantIdStr = principal.Claims.FirstOrDefault(c => c.Type == "tenantId")?.Value;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(tenantIdStr) || !int.TryParse(tenantIdStr, out var tenantId))
                return null;

            var usuario = await _usuarioProvider.ObterPorLogin(login);
            if (usuario == null || !usuario.Ativo || usuario.TenantId != tenantId)
                return null;

            var token = _jwtService.GerarJwt(usuario);
            var novoRefreshToken = _jwtService.GerarRefreshToken(usuario);

            return new TokenResponseDTO
            {
                Token = token,
                RefreshToken = novoRefreshToken,
                Expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos)
            };
        }
    }
} 