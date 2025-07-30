using System;
using ludusGestao.Eventos.Domain.Local;
using LudusGestao.Shared.Domain.Entities;

namespace ludusGestao.Eventos.Domain.Local.DTOs
{
    public class LocalDTO : DTOBase
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public string Endereco { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Cep { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public string Situacao { get; private set; }
        public int Capacidade { get; private set; }
        public decimal ValorHora { get; private set; }
        public Guid FilialId { get; private set; }

        // Construtor padrão necessário para o DTOBase
        public LocalDTO()
        {
        }

        public static LocalDTO Criar(Local local)
        {
            return new LocalDTO
            {
                Id = local.Id,
                Nome = local.Nome,
                Descricao = local.Descricao,
                Endereco = local.Endereco.Rua,
                Cidade = local.Endereco.Cidade,
                Estado = local.Endereco.Estado,
                Cep = local.Endereco.Cep,
                Telefone = local.Telefone.Numero,
                Situacao = local.Situacao.ToString(),
                TenantId = local.TenantId,
                DataCriacao = local.DataCriacao,
                DataAlteracao = local.DataAlteracao
            };
        }
    }
}