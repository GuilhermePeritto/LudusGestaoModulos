using System;

namespace ludusGestao.Autenticacao.Domain.Entities
{
    public class UsuarioAutenticacao
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public string Senha { get; set; } = default!;
        public int TenantId { get; set; }
        public bool Ativo { get; set; }
    }
} 