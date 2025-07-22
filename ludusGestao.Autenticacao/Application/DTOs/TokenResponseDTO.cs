namespace ludusGestao.Autenticacao.Application.DTOs
{
    public class TokenResponseDTO
    {
        public string Token { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime Expiracao { get; set; }
    }
} 