using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Autenticacao.Application.DTOs
{
    public class EntrarDTO
    {
        public Email Login { get; set; } = default!;
        public string Senha { get; set; } = default!;
    }
} 