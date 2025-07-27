using System;

namespace ludusGestao.Eventos.Domain.Entities.Local.DTOs
{
    public class LocalDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Cep { get; set; }
        public string Telefone { get; set; }
        public string Situacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public static LocalDTO Criar(ludusGestao.Eventos.Domain.Entities.Local.Local local)
        {
            return new LocalDTO
            {
                Id = local.Id,
                Nome = local.Nome,
                Descricao = local.Descricao,
                Rua = local.Endereco.Rua,
                Numero = local.Endereco.Numero,
                Bairro = local.Endereco.Bairro,
                Cidade = local.Endereco.Cidade,
                Estado = local.Endereco.Estado,
                Cep = local.Endereco.Cep,
                Telefone = local.Telefone.Numero,
                Situacao = local.Situacao.ToString(),
                DataCriacao = local.DataCriacao,
                DataAlteracao = local.DataAlteracao
            };
        }
    }
}