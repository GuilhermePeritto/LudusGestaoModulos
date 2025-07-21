using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Entities
{
    public class Filial : EntidadeBase, IEntidadeTenant
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public Endereco Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public Cnpj Cnpj { get; set; }
        public string Responsavel { get; set; }
        public SituacaoBase Situacao { get; set; }
        public DateTime DataAbertura { get; set; }
        public int TenantId { get; set; }
        public Guid EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
} 