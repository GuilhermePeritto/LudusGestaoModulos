using System.ComponentModel.DataAnnotations;

public class CriarLocalDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; private set; }
    [Required(ErrorMessage = "A rua é obrigatória.")]
    public string Rua { get; private set; }
    [Required(ErrorMessage = "O número é obrigatório.")]
    public string Numero { get; private set; }
    [Required(ErrorMessage = "O bairro é obrigatório.")]
    public string Bairro { get; private set; }
    [Required(ErrorMessage = "A cidade é obrigatória.")]
    public string Cidade { get; private set; }
    [Required(ErrorMessage = "O estado é obrigatório.")]
    public string Estado { get; private set; }
    [Required(ErrorMessage = "O CEP é obrigatório.")]
    public string Cep { get; private set; }
    [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser maior que zero.")]
    public int Capacidade { get; private set; }

    public static CriarLocalDTO Criar(Local local)
    {
        return new CriarLocalDTO
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