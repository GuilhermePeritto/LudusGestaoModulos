using System;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.DTOs.Filial
{
    public class AtualizarFilialDTO
    {
        public string Nome { get; set; }
        public string Codigo { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string Responsavel { get; set; }
        public DateTime DataAbertura { get; set; }
        public Endereco Endereco { get; set; }
        public Cnpj Cnpj { get; set; }
        public int Situacao { get; set; }
    }
} 