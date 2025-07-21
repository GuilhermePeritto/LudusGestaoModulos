using System;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Usuario
{
    public class AtualizarUsuarioDTO
    {
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Cargo { get; set; }
        public string? Foto { get; set; }
        public Email Email { get; set; }
        public Endereco Endereco { get; set; }
    }
} 