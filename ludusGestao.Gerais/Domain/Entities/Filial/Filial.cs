using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Filial
{
    public enum SituacaoFilial
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo
    }

    public class Filial : EntidadeBase
    {
        public string Nome { get; private set; }
        public Endereco Endereco { get; private set; }
        public Telefone Telefone { get; private set; }
        public Email Email { get; private set; }
        public Cnpj Cnpj { get; private set; }
        public string Responsavel { get; private set; }
        public SituacaoFilial Situacao { get; private set; }
        public DateTime DataAbertura { get; private set; }
        public Guid EmpresaId { get; private set; }

        public static Filial Criar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, string telefone, string email, string cnpj, string responsavel, DateTime dataAbertura, Guid empresaId)
        {
            return new Filial
            {
                Nome = nome,
                Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep),
                Telefone = new Telefone(telefone),
                Email = new Email(email),
                Cnpj = new Cnpj(cnpj),
                Responsavel = responsavel,
                DataAbertura = dataAbertura,
                EmpresaId = empresaId,
                Situacao = SituacaoFilial.Ativo
            };
        }

        public void Atualizar(string nome, string rua, string numero, string bairro, string cidade, string estado, string cep, string telefone, string email, string cnpj, string responsavel)
        {
            Nome = nome;
            Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep);
            Telefone = new Telefone(telefone);
            Email = new Email(email);
            Cnpj = new Cnpj(cnpj);
            Responsavel = responsavel;
        }

        public void Ativar()
        {
            Situacao = SituacaoFilial.Ativo;
        }

        public void Desativar()
        {
            Situacao = SituacaoFilial.Inativo;
        }

        public bool EstaAtiva()
        {
            return Situacao == SituacaoFilial.Ativo;
        }
    }
}