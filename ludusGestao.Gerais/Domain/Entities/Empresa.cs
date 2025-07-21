using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Entities
{
    public class Empresa : EntidadeBase, IEntidadeTenant
    {
        public string Nome { get; set; }
        public Cnpj Cnpj { get; set; }
        public string Email { get; set; }
        public Endereco Endereco { get; set; }
        public string Telefone { get; set; }
        public ICollection<Filial> Filiais { get; set; }
        public int TenantId { get; set; }
        public SituacaoBase Situacao { get; set; }
    }
} 