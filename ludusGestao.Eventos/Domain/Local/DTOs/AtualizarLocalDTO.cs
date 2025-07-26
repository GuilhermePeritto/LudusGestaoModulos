using System.ComponentModel.DataAnnotations;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.DTOs
{
    public class AtualizarLocalDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "A rua é obrigatória.")]
        public string Rua { get; set; }
        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        public string Cep { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
        public int Capacidade { get; set; }

        public static AtualizarLocalDTO Criar(Local local)
        {
            return new AtualizarLocalDTO
            {
                Nome = local.Nome,
                Rua = local.Endereco.Rua,
                Numero = local.Endereco.Numero,
                Bairro = local.Endereco.Bairro,
                Cidade = local.Endereco.Cidade,
                Estado = local.Endereco.Estado,
                Cep = local.Endereco.Cep,
                Capacidade = local.Capacidade
            };
        }
    }
}
