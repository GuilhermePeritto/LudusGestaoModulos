using System.Collections.Generic;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Empresa
{
    public enum SituacaoEmpresa
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo
    }

    public class Empresa : EntidadeBase
    {
        public string Nome { get; private set; }
        public Cnpj Cnpj { get; private set; }
        public Email Email { get; private set; }
        public Endereco Endereco { get; private set; }
        public Telefone Telefone { get; private set; }
        public SituacaoEmpresa Situacao { get; private set; }

        public static Empresa Criar(string nome, string cnpj, string email, string rua, string numero, string bairro, string cidade, string estado, string cep, string telefone)
        {
            return new Empresa
            {
                Nome = nome,
                Cnpj = new Cnpj(cnpj),
                Email = new Email(email),
                Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep),
                Telefone = new Telefone(telefone),
                Situacao = SituacaoEmpresa.Ativo
            };
        }

        public void Atualizar(string nome, string cnpj, string email, string rua, string numero, string bairro, string cidade, string estado, string cep, string telefone)
        {
            Nome = nome;
            Cnpj = new Cnpj(cnpj);
            Email = new Email(email);
            Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep);
            Telefone = new Telefone(telefone);
        }

        public void Ativar()
        {
            Situacao = SituacaoEmpresa.Ativo;
        }

        public void Desativar()
        {
            Situacao = SituacaoEmpresa.Inativo;
        }

        public bool EstaAtiva()
        {
            return Situacao == SituacaoEmpresa.Ativo;
        }
    }
}