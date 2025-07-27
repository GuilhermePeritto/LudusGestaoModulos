using System;
using LudusGestao.Shared.Domain.Entities;
using LudusGestao.Shared.Domain.ValueObjects;

namespace ludusGestao.Gerais.Domain.Usuario
{
    public enum SituacaoUsuario
    {
        Ativo = SituacaoBase.Ativo,
        Inativo = SituacaoBase.Inativo
    }

    public class Usuario : EntidadeBase
    {
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public Telefone Telefone { get; private set; }
        public string Cargo { get; private set; }
        public Guid EmpresaId { get; private set; }
        public SituacaoUsuario Situacao { get; private set; }
        public DateTime UltimoAcesso { get; private set; }
        public string? Foto { get; private set; }
        public string Senha { get; private set; }
        public Endereco Endereco { get; private set; }

        public static Usuario Criar(string nome, string email, string telefone, string cargo, Guid empresaId, string senha, string rua, string numero, string bairro, string cidade, string estado, string cep)
        {
            return new Usuario
            {
                Nome = nome,
                Email = new Email(email),
                Telefone = new Telefone(telefone),
                Cargo = cargo,
                EmpresaId = empresaId,
                Senha = senha,
                Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep),
                UltimoAcesso = DateTime.UtcNow,
                Situacao = SituacaoUsuario.Ativo
            };
        }

        public void Atualizar(string nome, string email, string telefone, string cargo, string rua, string numero, string bairro, string cidade, string estado, string cep)
        {
            Nome = nome;
            Email = new Email(email);
            Telefone = new Telefone(telefone);
            Cargo = cargo;
            Endereco = new Endereco(rua, numero, bairro, cidade, estado, cep);
        }

        public void AlterarSenha(string novaSenha)
        {
            Senha = novaSenha;
        }

        public void AtualizarUltimoAcesso()
        {
            UltimoAcesso = DateTime.UtcNow;
        }

        public void Ativar()
        {
            Situacao = SituacaoUsuario.Ativo;
        }

        public void Desativar()
        {
            Situacao = SituacaoUsuario.Inativo;
        }

        public bool EstaAtivo()
        {
            return Situacao == SituacaoUsuario.Ativo;
        }


    }
}