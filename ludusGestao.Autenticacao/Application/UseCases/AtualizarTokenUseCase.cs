using System;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.Services;
using ludusGestao.Autenticacao.Domain.Providers;
using ludusGestao.Autenticacao.Domain.Entities;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Application.Events;
using System.Collections.Generic;

namespace ludusGestao.Autenticacao.Application.UseCases
{
    public class AtualizarTokenUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AtualizarTokenUseCase(IUsuarioAutenticacaoReadProvider usuarioProvider, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponseDTO?> Executar(AtualizarTokenDTO dto)
        {
            var principal = _jwtService.ValidarJwt(dto.RefreshToken);
            if (principal == null)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "REFRESH_TOKEN_INVALIDO",
                    Mensagem = "Refresh token inválido.",
                    StatusCode = 400,
                    Erros = new List<string> { "Refresh token inválido." }
                });
                return null;
            }

            var tipoClaim = principal.Claims.FirstOrDefault(c => c.Type == "tipo")?.Value;
            if (tipoClaim != "refresh")
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "TIPO_TOKEN_INVALIDO",
                    Mensagem = "Tipo de token inválido.",
                    StatusCode = 400,
                    Erros = new List<string> { "Tipo de token inválido." }
                });
                return null;
            }

            var login = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value;
            var tenantIdStr = principal.Claims.FirstOrDefault(c => c.Type == "tenantId")?.Value;
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(tenantIdStr) || !int.TryParse(tenantIdStr, out var tenantId))
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "CLAIMS_INVALIDAS",
                    Mensagem = "Claims do token inválidas.",
                    StatusCode = 400,
                    Erros = new List<string> { "Claims do token inválidas." }
                });
                return null;
            }

            var usuario = await _usuarioProvider.ObterPorEmail(new LudusGestao.Shared.Domain.ValueObjects.Email(login));
            if (usuario == null)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "USUARIO_NAO_ENCONTRADO",
                    Mensagem = "Usuário não encontrado.",
                    StatusCode = 400,
                    Erros = new List<string> { "Usuário não encontrado." }
                });
                return null;
            }
            if (!usuario.Ativo)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "USUARIO_INATIVO",
                    Mensagem = "Usuário inativo.",
                    StatusCode = 403,
                    Erros = new List<string> { "Usuário inativo." }
                });
                return null;
            }
            if (usuario.TenantId != tenantId)
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "TENANT_INVALIDO",
                    Mensagem = "TenantId inválido.",
                    StatusCode = 400,
                    Erros = new List<string> { "TenantId inválido." }
                });
                return null;
            }

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