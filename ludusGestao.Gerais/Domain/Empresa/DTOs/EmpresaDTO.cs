using System;
using ludusGestao.Gerais.Domain.Empresa;

namespace ludusGestao.Gerais.Domain.Empresa.DTOs
{
    public class EmpresaDTO
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Cnpj { get; private set; }
        public string Email { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public string Telefone { get; private set; }
        public string Situacao { get; private set; }
        public int TenantId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }

        public static EmpresaDTO Criar(Empresa empresa)
        {
            return new EmpresaDTO
            {
                Id = empresa.Id,
                Nome = empresa.Nome,
                Cnpj = empresa.Cnpj.Numero,
                Email = empresa.Email,
                Rua = empresa.Endereco.Rua,
                Numero = empresa.Endereco.Numero,
                Bairro = empresa.Endereco.Bairro,
                Cidade = empresa.Endereco.Cidade,
                Estado = empresa.Endereco.Estado,
                Cep = empresa.Endereco.Cep,
                Telefone = empresa.Telefone,
                Situacao = empresa.Situacao.ToString(),
                TenantId = empresa.TenantId,
                DataCriacao = empresa.DataCriacao,
                DataAlteracao = empresa.DataAlteracao
            };
        }
    }
} 