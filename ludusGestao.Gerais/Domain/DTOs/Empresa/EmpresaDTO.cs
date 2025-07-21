using System;
using System.Collections.Generic;
using ludusGestao.Gerais.Domain.Enums;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Empresa
{
    public class EmpresaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public Cnpj Cnpj { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int TenantId { get; set; }
        public Endereco Endereco { get; set; }
        public SituacaoEmpresa Situacao { get; set; }
        public ICollection<Guid> Filiais { get; set; }
    }
} 