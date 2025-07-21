using System;
using LudusGestao.Shared.Domain.Entities;
using ludusGestao.Gerais.Domain.Enums;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Entities
{
    public class Usuario : EntidadeBase, IEntidadeTenant
    {
        public string Nome { get; set; }
        public Email Email { get; set; }
        public string Telefone { get; set; }
        public string Cargo { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
        public SituacaoUsuario Situacao { get; set; }
        public DateTime UltimoAcesso { get; set; }
        public string? Foto { get; set; }
        public string Senha { get; set; }
        public int TenantId { get; set; }
        public Endereco Endereco { get; set; }
    }
} 