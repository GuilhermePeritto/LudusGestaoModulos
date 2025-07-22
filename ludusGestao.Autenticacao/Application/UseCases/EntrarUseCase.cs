using System.Threading.Tasks;
using ludusGestao.Autenticacao.Application.DTOs;
using ludusGestao.Autenticacao.Application.Services;
using ludusGestao.Autenticacao.Domain.Providers;
using ludusGestao.Autenticacao.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace ludusGestao.Autenticacao.Application.UseCases
{
    public class EntrarUseCase
    {
        private readonly IUsuarioAutenticacaoReadProvider _usuarioProvider;
        private readonly JwtService _jwtService;

        public EntrarUseCase(IUsuarioAutenticacaoReadProvider usuarioProvider, JwtService jwtService)
        {
            _usuarioProvider = usuarioProvider;
            _jwtService = jwtService;
        }

        public async Task<TokenResponseDTO?> Executar(EntrarDTO dto)
        {
            var usuario = await _usuarioProvider.ObterPorLogin(dto.Login);
            if (usuario == null || !usuario.Ativo || !VerificarSenha(dto.Senha, usuario.Senha))
                return null;

            var token = _jwtService.GerarJwt(usuario);
            var refreshToken = _jwtService.GerarRefreshToken(usuario);

            return new TokenResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiracao = DateTime.UtcNow.AddMinutes(_jwtService.JwtExpiracaoMinutos)
            };
        }

        private bool VerificarSenha(string senha, string hash)
        {
            // Exemplo simples, substitua por BCrypt ou Argon2 em produção
            using var sha256 = SHA256.Create();
            var hashInput = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(senha)));
            return hashInput == hash;
        }
    }
} 