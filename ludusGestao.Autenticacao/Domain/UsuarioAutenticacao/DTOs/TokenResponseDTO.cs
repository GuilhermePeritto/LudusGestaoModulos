using System;
using ludusGestao.Autenticacao.Domain.UsuarioAutenticacao;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs
{
    public class TokenResponseDTO
    {
        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime Expiracao { get; private set; }
        public UsuarioAutenticacaoDTO Usuario { get; private set; }

        public static TokenResponseDTO Criar(string token, string refreshToken, DateTime expiracao, UsuarioAutenticacao usuario)
        {
            return new TokenResponseDTO
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiracao = expiracao,
                Usuario = UsuarioAutenticacaoDTO.Criar(usuario)
            };
        }
    }
} 