using System;
using ludusGestao.Gerais.Domain.Filial;

namespace ludusGestao.Gerais.Domain.Filial.DTOs
{
    public class FilialDTO
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Codigo { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Cnpj { get; private set; }
        public string Responsavel { get; private set; }
        public string Situacao { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public int TenantId { get; private set; }
        public Guid EmpresaId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }

        public static FilialDTO Criar(Filial filial)
        {
            return new FilialDTO
            {
                Id = filial.Id,
                Nome = filial.Nome,
                Codigo = filial.Codigo,
                Rua = filial.Endereco.Rua,
                Numero = filial.Endereco.Numero,
                Bairro = filial.Endereco.Bairro,
                Cidade = filial.Endereco.Cidade,
                Estado = filial.Endereco.Estado,
                Cep = filial.Endereco.Cep,
                Telefone = filial.Telefone.Numero,
                Email = filial.Email.Endereco,
                Cnpj = filial.Cnpj.Numero,
                Responsavel = filial.Responsavel,
                Situacao = filial.Situacao.ToString(),
                DataAbertura = filial.DataAbertura,
                TenantId = filial.TenantId,
                EmpresaId = filial.EmpresaId,
                DataCriacao = filial.DataCriacao,
                DataAlteracao = filial.DataAlteracao
            };
        }
    }
} 