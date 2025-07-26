using System;
using ludusGestao.Eventos.Domain.Local;

namespace ludusGestao.Eventos.Domain.Local.DTOs
{
    public class LocalDTO
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public int Capacidade { get; private set; }
        public bool Ativo { get; private set; }
        public int TenantId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public DateTime? DataAlteracao { get; private set; }

        public static LocalDTO Criar(Local local)
        {
            return new LocalDTO
            {
                Id = local.Id,
                Nome = local.Nome,
                Rua = local.Endereco.Rua,
                Numero = local.Endereco.Numero,
                Bairro = local.Endereco.Bairro,
                Cidade = local.Endereco.Cidade,
                Estado = local.Endereco.Estado,
                Cep = local.Endereco.Cep,
                Capacidade = local.Capacidade,
                Ativo = local.Ativo,
                TenantId = local.TenantId,
                DataCriacao = local.DataCriacao,
                DataAlteracao = local.DataAlteracao
            };
        }
    }
}