using System.Threading.Tasks;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.Services;
using ludusGestao.Autenticacao.Domain.Providers;
using ludusGestao.Autenticacao.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using LudusGestao.Shared.Application.Events;
using System.Collections.Generic;
using System;

namespace ludusGestao.Autenticacao.Application.UseCases
{
    public class EntrarUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EntrarUseCase(IUsuarioAutenticacaoReadProvider usuarioProvider, JwtService jwtService, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TokenResponseDTO?> Executar(EntrarDTO dto)
        {
            var usuario = await _usuarioProvider.ObterPorLogin(dto.Login);
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
            if (!VerificarSenha(dto.Senha, usuario.Senha))
            {
                await _httpContextAccessor.PublicarErro(new ErroEvento {
                    Codigo = "SENHA_INVALIDA",
                    Mensagem = "Senha inválida.",
                    StatusCode = 400,
                    Erros = new List<string> { "Senha inválida." }
                });
                return null;
            }

            var token = _jwtService.GerarJwt(usuario);
            var refreshToken = _jwtService.GerarRefreshToken(usuario);

            return new TokenResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos),
                Usuario = usuario
            };
        }

        private bool VerificarSenha(string senha, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(senha, hash);
        }
    }
} 