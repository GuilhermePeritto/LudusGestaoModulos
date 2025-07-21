using System;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Usuario
{
    public class CriarUsuarioDTO
    {
        public string Nome { get; set; }
        public Email Email { get; set; }
        public string Telefone { get; set; }
        public string Cargo { get; set; }
        public Guid EmpresaId { get; set; }
        public string? Foto { get; set; }
        public string Senha { get; set; }
        public int TenantId { get; set; }
        public Endereco Endereco { get; set; }
    }
} 