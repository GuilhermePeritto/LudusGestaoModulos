using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Empresa
{
    public class AtualizarEmpresaDTO
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public Cnpj Cnpj { get; set; }
        public int Situacao { get; set; }
        public Endereco Endereco { get; set; }
    }
} 