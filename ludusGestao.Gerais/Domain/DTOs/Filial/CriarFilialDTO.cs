using System;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Filial
{
    public class CriarFilialDTO
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public Cnpj Cnpj { get; set; }
        public string Responsavel { get; set; }
        public DateTime DataAbertura { get; set; }
        public int TenantId { get; set; }
        public Guid EmpresaId { get; set; }
        public Endereco Endereco { get; set; }
        public int Situacao { get; set; }
    }
} 