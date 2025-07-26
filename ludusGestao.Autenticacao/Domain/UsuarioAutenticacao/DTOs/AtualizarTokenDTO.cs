using System.ComponentModel.DataAnnotations;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs
{
    public class AtualizarTokenDTO
    {
        [Required(ErrorMessage = "O refresh token é obrigatório.")]
        public string RefreshToken { get; set; } = default!;
    }
} 