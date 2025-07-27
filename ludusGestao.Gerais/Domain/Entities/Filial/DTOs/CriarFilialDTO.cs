using System;
using System.ComponentModel.DataAnnotations;

namespace ludusGestao.Gerais.Domain.Filial.DTOs
{
    public class CriarFilialDTO
    {
        [Required(ErrorMessage = "O nome da filial é obrigatório.")]
        [StringLength(200, ErrorMessage = "O nome deve ter no máximo 200 caracteres.")]
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

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "O responsável é obrigatório.")]
        public string Responsavel { get; set; }

        [Required(ErrorMessage = "A data de abertura é obrigatória.")]
        public DateTime DataAbertura { get; set; }

        [Required(ErrorMessage = "O ID da empresa é obrigatório.")]
        public Guid EmpresaId { get; set; }
    }
} 