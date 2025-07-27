using System.ComponentModel.DataAnnotations;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Autenticacao.Domain.UsuarioAutenticacao.DTOs
{
    public class EntrarDTO
    {
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ter um formato válido.")]
        public string Email { get; set; } = default!;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string Senha { get; set; } = default!;
    }
} 